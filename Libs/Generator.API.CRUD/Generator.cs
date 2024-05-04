using System.Collections.Generic;
using System.Linq;
using D9bolic.Generator.API.CRUD.Providers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace D9bolic.Generator.API.CRUD
{
    /// <inheritdoc/>
    [Generator]
    public class Generator : ISourceGenerator
    {
        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
            /*
               if (!Debugger.IsAttached)
                 {
                     Debugger.Launch();
                 }
            */
            context.RegisterForSyntaxNotifications(() => new AnnotatedSyntaxReceiver());
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not AnnotatedSyntaxReceiver syntaxReceiver)
            {
                return;
            }

            if (!syntaxReceiver.Candidates.Any())
            {
                return;
            }

            var candidates = Map(context, syntaxReceiver.Candidates);

            ContextGenerator.Generate(context, candidates);
            RepositoryGenerator.Generate(context, candidates);
            DbContextFactoryGenerator.Generate(context, candidates);
            ServiceExtensionGenerator.Generate(context, candidates);
            foreach (var candidate in candidates)
            {
                Generate(context, candidate);
            }
        }

        private IEnumerable<ITypeSymbol> Map(GeneratorExecutionContext context,
            IEnumerable<ClassDeclarationSyntax> candidates)
        {
            return candidates.Select(candidate =>
                {
                    var model = context.Compilation.GetSemanticModel(candidate.SyntaxTree);
                    return model.GetDeclaredSymbol(candidate, context.CancellationToken) as ITypeSymbol;
                })
                .ToArray();
        }

        private void Generate(GeneratorExecutionContext context, ITypeSymbol candidate)
        {
            ServiceGenerator.Generate(context, candidate);
            ControllerGenerator.Generate(context, candidate);
        }
    }
}