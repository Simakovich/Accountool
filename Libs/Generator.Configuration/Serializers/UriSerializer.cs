using System;
using System.Collections.Generic;
using D9bolic.Generator.Configuration.Extensions;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.Configuration.Serializers;

public class UriSerializer : SerializerBase, ISerializationPlugin
{
    public int Priority => 0;

    public IEnumerable<string> Usings => Array.Empty<string>();

    public bool IsApplicable(ISymbol member, ITypeSymbol type) =>
        type.IsBaseClass("Uri", "System");

    public string ConstructValueGetter(ISymbol member, ITypeSymbol type, string valueAsStringProvider)
    {
        return
            $"string.IsNullOrEmpty({valueAsStringProvider}) ? {GenerateDefaultValue(member, type)} : new Uri({valueAsStringProvider})";
    }

    public string ConstructValueSetter(ISymbol member, ITypeSymbol type, string valueProvider)
    {
        return $"{valueProvider}.ToString()";
    }

    protected override string GenerateDefaultValueInternal(ITypeSymbol type, string value) => $"new Uri(@\"{value}\")";
}