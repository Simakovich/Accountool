using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using D9bolic.Generator.Configuration.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace D9bolic.Generator.Configuration
{
    [Generator]
    public class ConfigsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            /*
             if (!Debugger.IsAttached)
             {
                 Debugger.Launch();
             }
      */
#endif
            context.RegisterForSyntaxNotifications(() => new ConfigsSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ConfigsSyntaxReceiver syntaxReceiver)
            {
                return;
            }

            var plugins = GetPlugins(context);
            var sourcePlugins = GetPlugins<ISourcePlugin>(plugins);
            var serializationPlugins = GetPlugins<ISerializationPlugin>(plugins)
                .OrderByDescending(x => x.Priority)
                .ToArray();
            var candidates = Map(context, syntaxReceiver.ConfigCandidateTypes);
            var filteredCandidates = FilterConfigs(context, candidates, sourcePlugins, serializationPlugins);

            foreach (var candidate in filteredCandidates)
            {
                Generate(context, candidate);
            }
        }

        private void Generate(GeneratorExecutionContext context,
            (ITypeSymbol Interface, IEnumerable<ISourcePlugin> Sources, IEnumerable<ISerializationPlugin> Serializers)
                candidate)
        {
            var typeName =
                EscapeFileName(candidate.Interface!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
            var interfaceNamespace =
                candidate.Interface.ContainingNamespace?.ToDisplayString() ??
                "TMS.Generator.Configuration";
            var implementationName = $"{typeName}Implementation";

            var usings = string.Join(" ",
                candidate.Sources.SelectMany(plugin => plugin.Usings)
                    .Union(candidate.Serializers.SelectMany(plugin => plugin.Usings))
                    .Distinct()
                    .ToArray()
                    .Select(x => $"using {x};"));

            var dependencies = candidate.Sources.SelectMany(plugin => plugin.Dependencies).Distinct().ToArray();

            var fields = string.Join(" ", dependencies.Select(x => $"private readonly {x.Type} {x.Name};"));
            var arguments = string.Join(", ", dependencies.Select(x => $"{x.Type} {x.Name}"));
            var assignments = string.Join(" ", dependencies.Select(x => $"this.{x.Name} = {x.Name};"));
            var members = string.Join(" ",
                candidate.Sources.Select(plugin =>
                    plugin.Generate(context, candidate.Interface, candidate.Serializers)));

            var code = $@"
                {usings}
                namespace {interfaceNamespace};
                public partial class {implementationName} : {typeName}
                {{
                    {fields}

                    public {implementationName}({arguments})
                    {{
                       {assignments}
                    }}
                        
                    {members}
                }}
                ";

            var fileName = $"{EscapeFileName(candidate.Interface!.ToDisplayString())}.Implementation.g.cs"!;
            context.AddSource(fileName, Format(code));
        }

        private IEnumerable<Assembly> GetPlugins(GeneratorExecutionContext context)
        {
            var referenceAssemblies = context.Compilation.SourceModule.ReferencedAssemblySymbols;
            var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            return referenceAssemblies
                .Where(assembly =>
                {
                    var result = assembly.GetAttributes()
                        .Any(attributeData => attributeData.IsChildOf<ConfigPluginAttribute>());
                    return result;
                })
                .Select(assembly =>
                {
                    try
                    {
                        return Assembly.Load(new AssemblyName(assembly.Name));
                    }
                    catch (Exception ex)
                    {
                        Assembly? appDomainAssembly =
                            appDomainAssemblies.FirstOrDefault(a => a.GetName().Name == assembly.Name);
                        return appDomainAssembly;
                    }
                })
                .Where(assembly => assembly != null)
                .ToArray();
        }

        private IEnumerable<T> GetPlugins<T>(IEnumerable<Assembly> assemblies)
            where T : class
        {
            var types = Assembly
                .GetExecutingAssembly()
                .DefinedTypes
                .Where(type => type.IsPublic &&
                               !type.IsAbstract &&
                               !type.IsInterface &&
                               typeof(T).IsAssignableFrom(type))
                .ToList();

            var defaultSources = types
                .Select(type => Activator.CreateInstance(type) as T)
                .ToArray();

            return assemblies
                .SelectMany(assembly =>
                {
                    var types = assembly
                        .DefinedTypes
                        .Where(type =>
                            type.IsPublic && !type.IsAbstract && !type.IsInterface && typeof(T).IsAssignableFrom(type))
                        .ToList();
                    return types
                        .Select(type => Activator.CreateInstance(type) as T)
                        .ToArray();
                })
                .Union(defaultSources)
                .ToArray();
        }

        private IEnumerable<ITypeSymbol> Map(GeneratorExecutionContext context,
            IEnumerable<InterfaceDeclarationSyntax> candidates)
        {
            return candidates
                .Select(candidate =>
                {
                    var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
                    return model.GetDeclaredSymbol(candidate, context.CancellationToken) as ITypeSymbol;
                })
                .ToArray();
        }

        private IEnumerable<(ITypeSymbol Interface, IEnumerable<ISourcePlugin> Sources,
            IEnumerable<ISerializationPlugin> Serializers)> FilterConfigs(
            GeneratorExecutionContext context,
            IEnumerable<ITypeSymbol> candidates,
            IEnumerable<ISourcePlugin> sources,
            IEnumerable<ISerializationPlugin> serializers)
        {
            return candidates
                .Where(candidate =>
                {
                    return GetCandidateMembers(candidate).All
                        .All(member => member
                            .GetAttributes()
                            .Any(attribute =>
                                sources.Any(plugin => attribute.IsChildOf(plugin.Attribute))));
                })
                .Select(candidate =>
                {
                    var members = GetCandidateMembers(candidate);

                    var applicableSources = new HashSet<ISourcePlugin>();
                    var applicableSerializers = new HashSet<ISerializationPlugin>();

                    foreach (var property in members.Properties)
                    {
                        applicableSources.Add(sources.First(plugin =>
                            property.GetAttributes().Any(attribute => attribute.IsChildOf(plugin.Attribute))));

                        var serializer = serializers.FirstOrDefault(serializer =>
                            serializer.IsApplicable(property, property.Type));
                        if (serializer is not null)
                        {
                            applicableSerializers.Add(serializer);
                        }
                    }

                    foreach (var method in members.Methods)
                    {
                        applicableSources.Add(sources.First(plugin =>
                            method.GetAttributes().Any(attribute => attribute.IsChildOf(plugin.Attribute))));
                        var serializer = serializers.FirstOrDefault(serializer =>
                            serializer.IsApplicable(method, method.ReturnType));
                        if (serializer is not null)
                        {
                            applicableSerializers.Add(serializer);
                        }
                    }

                    return (candidate, applicableSources.AsEnumerable(), applicableSerializers.AsEnumerable());
                }).ToArray();
        }

        private static (IEnumerable<IMethodSymbol> Methods, IEnumerable<IPropertySymbol> Properties,
            IEnumerable<ISymbol> All) GetCandidateMembers(ITypeSymbol candidate)
        {
            var properties = candidate
                .GetMembers()
                .OfType<IPropertySymbol>()
                .ToArray();

            var propertyMethods = properties
                .Select(prop => prop.GetMethod)
                .Union(properties
                    .Select(prop => prop.SetMethod), SymbolEqualityComparer.IncludeNullability)
                .Where(method => method is not null)
                .ToArray();

            var methods = candidate
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(method => !propertyMethods.Contains(method, SymbolEqualityComparer.IncludeNullability))
                .ToArray();

            var members = properties.OfType<ISymbol>()
                .Union(methods.OfType<ISymbol>(), SymbolEqualityComparer.IncludeNullability).ToArray();

            return new(methods, properties, members);
        }

        private static string Format(string output)
        {
            var tree = CSharpSyntaxTree.ParseText(output);
            var root = (CSharpSyntaxNode) tree.GetRoot();
            output = root.NormalizeWhitespace(elasticTrivia: true).ToFullString();

            return output;
        }

        static string EscapeFileName(string fileName) => new[] {'<', '>', ','}
            .Aggregate(new StringBuilder(fileName), (s, c) => s.Replace(c, '_')).ToString();
    }
}