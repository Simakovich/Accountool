using System.Linq;
using D9bolic.Generator.Configuration.Extensions;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.Configuration.Serializers;

public class SerializerBase
{
    protected string GenerateDefaultValue(ISymbol member, ITypeSymbol type)
    {
        var attribute = member.GetAttributes().FirstOrDefault(attr => attr.IsChildOf(typeof(DefaultValueAttribute)));
        if (attribute is null)
        {
            return "default";
        }

        var value = attribute.ConstructorArguments.First().Value!.ToString();

        return GenerateDefaultValueInternal(type, value);
    }

    protected virtual string GenerateDefaultValueInternal(ITypeSymbol type, string value) => value;
}