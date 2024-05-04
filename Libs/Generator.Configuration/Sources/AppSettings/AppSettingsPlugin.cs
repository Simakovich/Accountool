using System;
using System.Collections.Generic;
using System.Linq;
using D9bolic.Generator.Configuration.Extensions;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.Configuration.Sources.AppSettings;

public class AppSettingsPlugin : ISourcePlugin
{
    public Type Attribute => typeof(AppSettingAttribute);

    public IEnumerable<string> Usings => new[]
    {
        "System",
        "Microsoft.Extensions.Configuration",
        "System.Text.Json",
        "System.Text.Json.Serialization",
    };

    public IEnumerable<Dependency> Dependencies => new[]
    {
        new Dependency
        {
            Type = "IConfiguration",
            Name = "_appSettingsConfiguration",
        }
    };

    public string Generate(GeneratorExecutionContext context, ITypeSymbol candidate,
        IEnumerable<ISerializationPlugin> serializers)
    {
        var members = candidate
            .GetMembers()
            .Where(x => x.GetAttributes().Any(attr => attr.IsChildOf(typeof(AppSettingAttribute))))
            .ToArray();

        foreach (var method in members.OfType<IMethodSymbol>())
        {
            foreach (var location in method.Locations)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "AC10000",
                        $"AppSettings attribute is defined for method {method.Name} of class {method.ContainingSymbol.Name}",
                        $"AppSettings attribute is defined for method {method.Name} of class {method.ContainingSymbol.Name}",
                        "AppSettings config generation",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                    location));
            }
        }

        var properties = members
            .OfType<IPropertySymbol>()
            .Select(prop =>
                GenerateProperty(context, prop, serializers.FirstOrDefault(x => x.IsApplicable(prop, prop.Type))))
            .ToArray();

        return string.Join(" ", properties);
    }

    private string GenerateProperty(GeneratorExecutionContext context, IPropertySymbol property,
        ISerializationPlugin? serializer)
    {
        if (property.SetMethod != null)
        {
            foreach (var location in property.Locations)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "AC10000",
                        $"AppSettings setter defined for property {property.Name} of class {property.ContainingSymbol.Name}",
                        $"AppSettings setter defined for property {property.Name} of class {property.ContainingSymbol.Name}",
                        "AppSettings config generation",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                    location));
            }

            return string.Empty;
        }

        var attribute = property.GetAttributes().First(attr => attr.IsChildOf(typeof(AppSettingAttribute)));
        var key = attribute.ConstructorArguments.First().Value!.ToString().Replace(".", ":");
        return serializer is null
            ? GenerateDefaultProperty(key, property)
            : $"{GetAccessModifier(property)} {property.Type} {property.Name} => {serializer?.ConstructValueGetter(property, property.Type, $"_appSettingsConfiguration[\"{key}\"]")};";
    }

    private string GetAccessModifier(IPropertySymbol property)
    {
        return property.ExplicitInterfaceImplementations.Any() ? string.Empty : "public";
    }

    private string GenerateDefaultProperty(string key, IPropertySymbol property)
    {
        switch (property.Type.SpecialType)
        {
            case SpecialType.System_Object:
            case SpecialType.System_Array:
            case SpecialType.None when property.Type.IsReferenceType:
                return
                    $"{GetAccessModifier(property)} {property.Type} {property.Name} => _appSettingsConfiguration.GetSection(\"{key}\").Get<{property.Type}>() ?? {GenerateDefaultValue(property)};";

            case SpecialType.System_Enum:
            case SpecialType.None when property.Type.BaseType.SpecialType == SpecialType.System_Enum:
                return $@"{GetAccessModifier(property)} {property.Type} {property.Name}
                           {{
                              get  
                              {{
                                  if (Enum.TryParse<{property.Type}>(_appSettingsConfiguration[""{key}""], out var value))
                                  {{
                                        return value;
                                  }}
                                
                                  return {GenerateDefaultValue(property)};  
                               }}
                           }}";
            case SpecialType.System_String:
                return
                    $"{GetAccessModifier(property)} {property.Type} {property.Name} => _appSettingsConfiguration[\"{key}\"] ?? {GenerateDefaultValue(property)};";
            default:
                return
                    $"{GetAccessModifier(property)} {property.Type} {property.Name} => {property.Type}.Parse(_appSettingsConfiguration[\"{key}\"]);";
        }
    }

    private string GenerateDefaultValue(IPropertySymbol property)
    {
        var attribute = property.GetAttributes()
            .FirstOrDefault(attr =>
                attr.IsChildOf(typeof(DefaultValueAttribute)));
        if (attribute is null)
        {
            return "default";
        }

        var value = attribute.ConstructorArguments.First().Value!.ToString();

        return property.Type.SpecialType switch
        {
            SpecialType.System_String => $"@\"{value}\"",
            SpecialType.System_Boolean => value.ToLower(),
            SpecialType.System_Enum => $"({property.Type}){value}",
            SpecialType.None when property.Type.BaseType?.SpecialType == SpecialType.System_Enum =>
                $"({property.Type}){value}",
            _ => value
        };
    }
}