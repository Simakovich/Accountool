using System.Collections.Generic;
using D9bolic.Generator.API.CRUD.Utils;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.API.CRUD.Providers;

public static class DbContextFactoryGenerator
{
    public static void Generate(GeneratorExecutionContext context, IEnumerable<ITypeSymbol> candidates)
    {
        var assemblyName = context.Compilation.AssemblyName;

        var code = @$"
            using Microsoft.EntityFrameworkCore;
            
            namespace {assemblyName}.DataAccess;
        
            public class DbContextFactory : D9bolic.EntityFramework.DbContextScope.Interfaces.IDbContextFactory
            {{
                private readonly Action<DbContextOptionsBuilder> _options;
        
                public DbContextFactory(Action<DbContextOptionsBuilder> options)
                {{
                    _options = options;
                }}
                    
                public TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext
                {{
                    var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
                    _options(optionsBuilder);
                    return ((TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options));
                }}
            }}";

        var fileName = $" {assemblyName}.DataAccess.ContextFactory.g.cs"!;
        context.AddSource(fileName, code.FormatCode());
    }
}