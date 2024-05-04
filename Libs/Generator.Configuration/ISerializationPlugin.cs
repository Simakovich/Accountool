using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.Configuration;

public interface ISerializationPlugin
{
    int Priority { get; }

    IEnumerable<string> Usings { get; }

    bool IsApplicable(ISymbol member, ITypeSymbol type);

    string ConstructValueGetter(ISymbol member, ITypeSymbol type, string valueAsStringProvider);

    string ConstructValueSetter(ISymbol member, ITypeSymbol type, string valueProvider);
}