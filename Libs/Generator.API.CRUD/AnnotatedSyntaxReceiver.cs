using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace D9bolic.Generator.API.CRUD;

/// <inheritdoc/>
public class AnnotatedSyntaxReceiver : ISyntaxReceiver
{
    private static readonly string AttributeName = "Table";

    /// <summary>
    /// Candidates for the custom options provider.
    /// </summary>
    public HashSet<ClassDeclarationSyntax> Candidates { get; } = new();

    /// <summary>
    /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation.
    /// </summary>
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax declaration)
        {
            return;
        }

        CheckCandidateSyntax(declaration);
    }

    private void CheckCandidateSyntax(ClassDeclarationSyntax declaration)
    {
        if (IsAnnotated(declaration))
        {
            Candidates.Add(declaration);
        }
    }

    private bool IsAnnotated(MemberDeclarationSyntax node)
    {
        return node.AttributeLists
            .SelectMany(attributeListSyntax => attributeListSyntax.Attributes)
            .Any(attributeSyntax => attributeSyntax.Name.NormalizeWhitespace().ToFullString() == AttributeName);
    }
}