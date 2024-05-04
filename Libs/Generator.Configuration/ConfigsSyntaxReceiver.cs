using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace D9bolic.Generator.Configuration
{
    /// <summary>
    /// Created on demand before each generation pass
    /// </summary>
    internal class ConfigsSyntaxReceiver : ISyntaxReceiver
    {
        public HashSet<InterfaceDeclarationSyntax> ConfigCandidateTypes { get; } = new();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not InterfaceDeclarationSyntax interfaceDeclaration)
            {
                return;
            }

            CheckCandidateSyntax(interfaceDeclaration);
        }

        private void CheckCandidateSyntax(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            var members = interfaceDeclaration.Members;
            var isApplied = members.Any() && members
                .All(property => property.AttributeLists.SelectMany(x => x.Attributes).Any());

            if (isApplied)
            {
                ConfigCandidateTypes.Add(interfaceDeclaration);
            }
        }
    }
}