using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace D9bolic.Generator.API.CRUD.Providers;

public static class DependenciesProvider
{
    public class Dependency
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }

    public static IEnumerable<ITypeSymbol> GetPartialClasses(GeneratorExecutionContext context, string typeName,
        string @namespace)
    {
        return context.Compilation
            .SyntaxTrees
            .SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
            .Where(x => x is ClassDeclarationSyntax)
            .Cast<ClassDeclarationSyntax>()
            .Where(c => c.Identifier.ValueText.Equals(typeName, StringComparison.InvariantCulture))
            .Select(c =>
            {
                var model = context.Compilation.GetSemanticModel(c.SyntaxTree);
                return model.GetDeclaredSymbol(c, context.CancellationToken) as ITypeSymbol;
            })
            .Where(c => c is not null)
            .Where(c => c.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                .Equals($"global::{@namespace}"))
            .ToImmutableList();
    }

    public static IEnumerable<Dependency> GetDependencies(GeneratorExecutionContext context, string typeName,
        string @namespace)
    {
        var partialClasses = GetPartialClasses(context, typeName, @namespace);
        return GetDependencies(partialClasses);
    }

    public static string GetConstructorArguments(this IEnumerable<Dependency> dependencies)
    {
        if (!dependencies.Any())
        {
            return string.Empty;
        }

        return "," + string.Join(", ", dependencies.Select(dependency => $"{dependency.Type} {dependency.Name}"));
    }

    public static IEnumerable<IMethodSymbol> GetMethods(this IEnumerable<ITypeSymbol> partialClasses)
    {
        return partialClasses.SelectMany(x => x.GetMembers().OfType<IMethodSymbol>()).DistinctBy(x => x.Name).ToArray();
    }

    public static IEnumerable<Dependency> GetDependencies(this IEnumerable<ITypeSymbol> partialClasses)
    {
        return partialClasses.SelectMany(x =>
        {
            var props = x.GetMembers().OfType<IPropertySymbol>().Where(p => p.SetMethod != null)
                .Select(x => new Dependency
                {
                    Name = x.Name,
                    Type = x.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                });
            var fields = x.GetMembers().OfType<IFieldSymbol>().Select(x => new Dependency
            {
                Name = x.Name,
                Type = x.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            });
            return props.Union(fields).ToList();
        }).DistinctBy(x => x.Name).ToArray();
    }

    public static string GetConstructorAssigment(this IEnumerable<Dependency> dependencies)
    {
        if (!dependencies.Any())
        {
            return string.Empty;
        }

        return string.Join("\r\n", dependencies.Select(dependency => $" this.{dependency.Name}={dependency.Name};"));
    }


    /// <summary>Returns distinct elements from a sequence according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>An <see cref="IEnumerable{T}" /> that contains distinct elements from the source sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its `GetEnumerator` method directly or by using `foreach` in Visual C# or `For Each` in Visual Basic.</para>
    /// <para>The <see cref="DistinctBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})" /> method returns an unordered sequence that contains no duplicate values. The default equality comparer, <see cref="EqualityComparer{T}.Default" />, is used to compare values.</para>
    /// </remarks>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector) => DistinctBy(source, keySelector, null);

    /// <summary>Returns distinct elements from a sequence according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}" /> to compare keys.</param>
    /// <returns>An <see cref="IEnumerable{T}" /> that contains distinct elements from the source sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <remarks>
    /// <para>This method is implemented by using deferred execution. The immediate return value is an object that stores all the information that is required to perform the action. The query represented by this method is not executed until the object is enumerated either by calling its `GetEnumerator` method directly or by using `foreach` in Visual C# or `For Each` in Visual Basic.</para>
    /// <para>The <see cref="DistinctBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}?)" /> method returns an unordered sequence that contains no duplicate values. If <paramref name="comparer" /> is <see langword="null" />, the default equality comparer, <see cref="EqualityComparer{T}.Default" />, is used to compare values.</para>
    /// </remarks>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (keySelector is null)
        {
            throw new ArgumentNullException(nameof(keySelector));
        }

        return DistinctByIterator(source, keySelector, comparer);
    }

    private static IEnumerable<TSource> DistinctByIterator<TSource, TKey>(IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
    {
        using IEnumerator<TSource> enumerator = source.GetEnumerator();

        if (enumerator.MoveNext())
        {
            var set = new HashSet<TKey>(comparer);
            do
            {
                TSource element = enumerator.Current;
                if (set.Add(keySelector(element)))
                {
                    yield return element;
                }
            } while (enumerator.MoveNext());
        }
    }
}