using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.API.CRUD.Utils
{
    /// <summary>
    /// Represent type symbols processing extensions.
    /// </summary>
    public static class INamedTypeSymbolExtensions
    {
        public static IEnumerable<string> GetProps(this IEnumerable<INamedTypeSymbol> types, string name,
            string @namespace)
        {
            return types.Where(type =>
                {
                    Logger.WriteInfo("--------------------------------------------------------------------");
                    Logger.WriteInfo(type.ContainingNamespace.Name);
                    Logger.WriteInfo(type.Name);
                    Logger.WriteInfo("Result: " +
                                     (type.ContainingNamespace.Name.Equals(@namespace) && type.Name.Equals(name)));
                    return type.ContainingNamespace.Name.Equals(@namespace) && type.Name.Equals(name);
                })
                .SelectMany(type => type.GetMembers().OfType<IPropertySymbol>())
                .Where(prop => prop.SetMethod is not null)
                .Select(prop => prop.Name);
        }

        /// <summary>
        /// Check type for the inheritance.
        /// </summary>
        /// <param name="typeSymbol">Checked type symbols.</param>
        /// <param name="typeToCheck">Possible base class.</param>
        /// <returns>True if inheritor and false if not.</returns>
        public static bool IsBaseClass(this ITypeSymbol typeSymbol, Type typeToCheck)
        {
            return typeSymbol.IsBaseClass(typeToCheck.Name, typeToCheck.Namespace);
        }

        /// <summary>
        /// Check type for the inheritance.
        /// </summary>
        /// <param name="typeSymbol">Checked type symbols.</param>
        /// <param name="typeToCheck">Possible base class name.</param>
        /// <param name="nameSpace">Possible base class namespace.</param>
        /// <returns>True if inheritor and false if not.</returns>
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

            return (typeSymbol.BaseType is not null && typeSymbol.BaseType!.MetadataName == typeToCheck) ||
                   typeSymbol.BaseType.IsBaseClass(typeToCheck, nameSpace);
        }

        public static IEnumerable<INamedTypeSymbol> AllNestedTypesAndSelf(this INamedTypeSymbol type)
        {
            yield return type;
            foreach (var typeMember in type.GetTypeMembers())
            {
                foreach (var nestedType in typeMember.AllNestedTypesAndSelf())
                {
                    yield return nestedType;
                }
            }
        }
    }
}