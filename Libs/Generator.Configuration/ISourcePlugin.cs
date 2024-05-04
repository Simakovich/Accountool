using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.Configuration;

public interface ISourcePlugin
{
    Type Attribute { get; }

    IEnumerable<string> Usings { get; }

    IEnumerable<Dependency> Dependencies { get; }

    public string Generate(GeneratorExecutionContext context, ITypeSymbol candidate,
        IEnumerable<ISerializationPlugin> serializers);
}

public class Dependency
{
    protected bool Equals(Dependency other)
    {
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Dependency) obj);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public string Type { get; set; }

    public string Name { get; set; }
}