using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.Configuration.Extensions
{
    public static class AttributeDataExtensions
    {
        public static bool IsChildOf(this ITypeSymbol checkedType, Type type)
            => checkedType.IsBaseClass(type);

        public static bool IsChildOf(this AttributeData checkedType, ITypeSymbol type)
            => checkedType.AttributeClass.IsBaseClass(type);

        public static bool IsChildOf(this AttributeData attributeData, Type type)
            => attributeData.AttributeClass.IsBaseClass(type);

        public static bool IsChildOf<TType>(this AttributeData attributeData)
            => attributeData.AttributeClass.IsBaseClass(typeof(TType));

        public static bool IsChildOfAny(this AttributeData attributeData, IEnumerable<ITypeSymbol> types)
            => types.Any(type => attributeData.AttributeClass.IsBaseClass(type));

        public static IEnumerable<object> GetActualConstructorParams(this AttributeData attributeData)
        {
            foreach (var argument in attributeData.ConstructorArguments)
            {
                if (argument.Kind == TypedConstantKind.Array)
                {
                    // Assume they are strings, but the array that we get from this
                    // should actually be of type of the objects within it, be it strings or ints
                    // This is definitely possible with reflection, I just don't know how exactly. 
                    yield return argument.Values.Select(a => a.Value).OfType<string>().ToArray();
                }
                else
                {
                    yield return $"\"{argument.Value}\"";
                }
            }
        }
    }
}