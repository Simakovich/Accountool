using System;
using System.Collections.Generic;
using System.Linq;
using D9bolic.Generator.Configuration.Extensions;
using D9bolic.Generator.Configuration.Serializers;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.Configuration.Sources.Environment;

public class EnvironmentVariablePlugin : ISourcePlugin
{
    private static IEnumerable<ISerializationPlugin> DefaultSerializers = new ISerializationPlugin[]
    {
        new ObjectSerializer(),
        new StringSerializer(),
        new BooleanSerializer(),
        new EnumSerializer(),
        new PrimitivesSerializer()
    };

    public Type Attribute => typeof(EnvironmentVariableAttribute);

    public IEnumerable<string> Usings => new[]
    {
        "System",
        "System.Text.Json",
        "System.Text.Json.Serialization",
    };

    public IEnumerable<Dependency> Dependencies => Array.Empty<Dependency>();

    public string Generate(GeneratorExecutionContext context, ITypeSymbol candidate,
        IEnumerable<ISerializationPlugin> serializers)
    {
        var all = serializers.Union(DefaultSerializers).ToArray();
        var members = candidate
            .GetMembers()
            .Where(x => x.GetAttributes().Any(attr => attr.IsChildOf(typeof(EnvironmentVariableAttribute))))
            .ToArray();

        foreach (var method in members.OfType<IMethodSymbol>())
        {
            foreach (var location in method.Locations)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "AC10000",
                        $"EnvironmentVariable attribute defined for method {method.Name} of class {method.ContainingSymbol.Name}",
                        $"EnvironmentVariable attribute defined for method {method.Name} of class {method.ContainingSymbol.Name}",
                        "EnvironmentVariable config generation",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                    location));
            }
        }

        var properties = members
            .OfType<IPropertySymbol>()
            .Select(prop => GenerateProperty(context, prop, all.First(x => x.IsApplicable(prop, prop.Type))))
            .ToArray();

        return string.Join(" ", properties);
    }

    private string GenerateProperty(GeneratorExecutionContext context, IPropertySymbol property,
        ISerializationPlugin serializer)
    {
        var attribute = property.GetAttributes()
            .First(attr => attr.IsChildOf(typeof(EnvironmentVariableAttribute)));

        var key = $"\"{context.Compilation.AssemblyName}.{attribute.ConstructorArguments.First().Value!.ToString()}\"";
        return @$"{GetAccessModifier(property)} {property.Type} {property.Name} 
                                    {{
                                        {GenerateGetter(property, key, serializer)}
                                        {GenerateSetter(property, key, serializer)}
                                    }}";
    }

    private string GenerateSetter(IPropertySymbol property, string key, ISerializationPlugin serializer)
    {
        if (property.SetMethod is null)
        {
            return string.Empty;
        }

        return
            $"set {{System.Environment.SetEnvironmentVariable(${key}, {serializer.ConstructValueSetter(property, property.Type, "value")}, EnvironmentVariableTarget.Machine);}}";
    }

    private string GenerateGetter(IPropertySymbol property, string key, ISerializationPlugin serializer)
    {
        return property.GetMethod is null
            ? string.Empty
            : $"get {{ return {serializer.ConstructValueGetter(property, property.Type, $"System.Environment.GetEnvironmentVariable(${key}, EnvironmentVariableTarget.Machine)")};}}";
    }

    private string GetAccessModifier(IPropertySymbol property)
    {
        return property.ExplicitInterfaceImplementations.Any() ? string.Empty : "public";
    }

    private class BooleanSerializer : SerializerBase, ISerializationPlugin
    {
        public int Priority => int.MinValue;

        public IEnumerable<string> Usings => Array.Empty<string>();

        public bool IsApplicable(ISymbol member, ITypeSymbol type) =>
            type.SpecialType == SpecialType.System_Boolean;

        public string ConstructValueGetter(ISymbol member, ITypeSymbol type, string valueAsStringProvider) =>
            $"{type}.TryParse({valueAsStringProvider}?.ToLower(), out var value) ? value : {GenerateDefaultValue(member, type)}";

        public string ConstructValueSetter(ISymbol member, ITypeSymbol type, string valueProvider) =>
            $"{valueProvider}.ToString().ToLower()";

        protected override string GenerateDefaultValueInternal(ITypeSymbol type, string value) =>
            value.ToLower();
    }

    private class EnumSerializer : SerializerBase, ISerializationPlugin
    {
        public int Priority => int.MinValue;

        public IEnumerable<string> Usings => Array.Empty<string>();

        public bool IsApplicable(ISymbol member, ITypeSymbol type) =>
            type.SpecialType == SpecialType.System_Enum ||
            type.SpecialType == SpecialType.None && type.BaseType?.SpecialType == SpecialType.System_Enum;


        public string ConstructValueGetter(ISymbol member, ITypeSymbol type, string valueAsStringProvider) =>
            $"Enum.TryParse<{type}>({valueAsStringProvider}, out var value) ? value : {GenerateDefaultValue(member, type)}";

        public string ConstructValueSetter(ISymbol member, ITypeSymbol type, string valueProvider) =>
            $"{valueProvider}.ToString()";

        protected override string GenerateDefaultValueInternal(ITypeSymbol type, string value) =>
            $"({type}){value}";
    }

    private class ObjectSerializer : SerializerBase, ISerializationPlugin
    {
        public IEnumerable<string> Usings => new[]
        {
            "System",
            "System.Text.Json",
            "System.Text.Json.Serialization",
        };

        public int Priority => int.MinValue;

        public bool IsApplicable(ISymbol member, ITypeSymbol type) =>
            type.SpecialType == SpecialType.System_Object ||
            type.SpecialType == SpecialType.System_Array ||
            (type.SpecialType == SpecialType.None && type.IsReferenceType);


        public string ConstructValueGetter(ISymbol member, ITypeSymbol type, string valueAsStringProvider)
        {
            return
                $"JsonSerializer.Deserialize<{type}>({valueAsStringProvider}) ?? {GenerateDefaultValue(member, type)}";
        }

        public string ConstructValueSetter(ISymbol member, ITypeSymbol type, string valueProvider)
        {
            return $"JsonSerializer.Serialize({valueProvider}))";
        }
    }

    private class PrimitivesSerializer : SerializerBase, ISerializationPlugin
    {
        private static readonly IEnumerable<SpecialType> Primitives = new[]
        {
            SpecialType.System_Char,
            SpecialType.System_Byte,
            SpecialType.System_Decimal,
            SpecialType.System_Double,
            SpecialType.System_Int16,
            SpecialType.System_Int32,
            SpecialType.System_Int64,
            SpecialType.System_Single,
            SpecialType.System_DateTime,
            SpecialType.System_ValueType,
        };

        public int Priority => int.MinValue;

        public IEnumerable<string> Usings => Array.Empty<string>();

        public bool IsApplicable(ISymbol member, ITypeSymbol type) =>
            (Primitives.Contains(type.SpecialType) ||
             (type.SpecialType == SpecialType.None && type.BaseType?.SpecialType == SpecialType.System_ValueType)) &&
            !(type.SpecialType == SpecialType.System_Enum ||
              type.SpecialType == SpecialType.None && type.BaseType?.SpecialType == SpecialType.System_Enum);


        public string ConstructValueGetter(ISymbol member, ITypeSymbol type, string valueAsStringProvider)
        {
            return
                $"{type}.TryParse({valueAsStringProvider}, out var value) ? value : {GenerateDefaultValue(member, type)}";
        }

        public string ConstructValueSetter(ISymbol member, ITypeSymbol type, string valueProvider) =>
            $"{valueProvider}.ToString()";
    }

    private class StringSerializer : SerializerBase, ISerializationPlugin
    {
        public int Priority => int.MinValue;

        public IEnumerable<string> Usings => Array.Empty<string>();

        public bool IsApplicable(ISymbol member, ITypeSymbol type) =>
            type.SpecialType == SpecialType.System_String;

        public string ConstructValueGetter(ISymbol member, ITypeSymbol type, string valueAsStringProvider)
        {
            return $"{valueAsStringProvider} ?? {GenerateDefaultValue(member, type)}";
        }

        public string ConstructValueSetter(ISymbol member, ITypeSymbol type, string valueProvider) => valueProvider;

        protected override string GenerateDefaultValueInternal(ITypeSymbol type, string value) => $"@\"{value}\"";
    }
}