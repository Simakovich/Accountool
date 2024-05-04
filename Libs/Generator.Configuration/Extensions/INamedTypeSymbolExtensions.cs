using System;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.Configuration.Extensions
{
    public static class INamedTypeSymbolExtensions
    {
        public static bool IsBaseClass(this ITypeSymbol typeSymbol, string typeToCheck, string nameSpace)
        {
            if (typeSymbol == null)
            {
                return false;
            }

            if (typeSymbol.MetadataName == typeToCheck &&
                (typeSymbol.ContainingNamespace?.ToDisplayString().Equals(nameSpace) ?? true))
            {
                return true;
            }

            return typeSymbol.BaseType is not null && typeSymbol.BaseType!.MetadataName == typeToCheck ||
                   typeSymbol.BaseType.IsBaseClass(typeToCheck, nameSpace);
        }

        public static bool IsBaseClass(this ITypeSymbol typeSymbol, Type typeToCheck)
        {
            return typeSymbol.IsBaseClass(typeToCheck.Name, typeToCheck.Namespace);
        }

        public static bool IsBaseClass(this ITypeSymbol typeSymbol, ITypeSymbol typeToCheck)
        {
            return typeSymbol.IsBaseClass(typeToCheck.Name, typeToCheck.ContainingNamespace?.ToDisplayString());
        }
    }
}