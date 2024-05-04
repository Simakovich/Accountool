using System.Collections.Generic;
using System.Linq;
using D9bolic.Generator.API.CRUD.Utils;
using Humanizer;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.API.CRUD.Providers;

public static class ContextGenerator
{
    public static void Generate(GeneratorExecutionContext context, IEnumerable<ITypeSymbol> candidates)
    {
        var assemblyName = context.Compilation.AssemblyName;

        var namespaces = candidates
            .Select(x => x.ContainingNamespace.ToString())
            .Distinct()
            .Select(x => $"using {x};");
        var code = @$"using System.Data;
                  {string.Join(" ", namespaces)}
                  using D9bolic.EntityFramework.DbContextScope.Interfaces;
                  using Microsoft.EntityFrameworkCore;
  
                  namespace {assemblyName}.DataAccess;    

                  public class ApplicationsDbContext : DbContext
                  {{
                      public ApplicationsDbContext(DbContextOptions<ApplicationsDbContext> options)
                      : base(options)
                      {{
                      }}
          
                      {GenerateDbSets(context, candidates)}
                  }}";

        var fileName = $" {assemblyName}.DataAccess.Context.g.cs"!;
        context.AddSource(fileName, code.FormatCode());
    }

    private static string GenerateDbSets(GeneratorExecutionContext context, IEnumerable<ITypeSymbol> candidates)
    {
        return string.Join(" ", candidates
            .Select(candidate =>
            {
                var typeName =
                    candidate!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).EscapeFileName();
                var pluralName = typeName.Pluralize();
                return $"public DbSet<{typeName}> {pluralName} {{ get; set; }} = null!;";
            }));
    }
}