using System.Collections.Generic;
using System.Linq;
using D9bolic.Generator.API.CRUD.Utils;
using Humanizer;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.API.CRUD.Providers;

public class ServiceExtensionGenerator
{
    public static void Generate(GeneratorExecutionContext context, IEnumerable<ITypeSymbol> candidates)
    {
        var assemblyName = context.Compilation.AssemblyName;

        var code = @$"using Microsoft.Extensions.DependencyInjection.Extensions;
                  using D9bolic.EntityFramework.DbContextScope.Implementations;
                  using D9bolic.EntityFramework.DbContextScope.Interfaces;
                  using Microsoft.EntityFrameworkCore;
                  using {assemblyName}.Services;
                  using {assemblyName}.DataAccess;
  
                  namespace {assemblyName}.Extensions;    

                  public static class ServiceCollectionExtensions
                  {{
                        public static IServiceCollection AddGeneratedCRUD(this IServiceCollection services, Action<DbContextOptionsBuilder> contextOptionsAction)
                        {{
                            services.AddDbContext<ApplicationsDbContext>(contextOptionsAction);
                            services.AddSingleton<D9bolic.EntityFramework.DbContextScope.Interfaces.IDbContextFactory, DbContextFactory>();
                            services.AddSingleton<Action<DbContextOptionsBuilder>>(contextOptionsAction);
                            services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
                            services.AddSingleton<DbContextScopeFactory>();
                            services.AddSingleton<DbContextScopeFactoryDispatcher>();
                            services.AddSingleton<IDbContextScopeFactory>(x => x.GetService<DbContextScopeFactoryDispatcher>());
                            services.AddSingleton<IDbContextScopeContainer>(x => x.GetService<DbContextScopeFactoryDispatcher>());
                            services.AddScoped(typeof(IRepository<>), typeof(GenericSqlRepository<>));
                           
                            {GenerateServicesRegistration(context, candidates)}

                            return services;
                        }}
                  }}";

        var fileName = $" {assemblyName}.Extensions.ServiceCollection.g.cs"!;
        context.AddSource(fileName, code.FormatCode());
    }

    private static string GenerateServicesRegistration(GeneratorExecutionContext context,
        IEnumerable<ITypeSymbol> candidates)
    {
        return string.Join(" ", candidates
            .Select(candidate =>
            {
                var typeName =
                    candidate!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).EscapeFileName();
                var pluralName = typeName.Pluralize();
                return $"services.AddScoped< I{typeName}Service, {typeName}Service>();";
            }));
    }
}