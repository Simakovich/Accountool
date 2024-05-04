using System.Collections.Generic;
using System.Linq;
using D9bolic.Generator.API.CRUD.Utils;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.API.CRUD.Providers;

public static class ServiceGenerator
{
    public static void Generate(GeneratorExecutionContext context, ITypeSymbol candidate)
    {
        var assemblyName = context.Compilation.AssemblyName;
        var @namespace = $"{assemblyName}.Services";
        var typeName =
            candidate!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).EscapeFileName();

        var partialClasses = DependenciesProvider.GetPartialClasses(context, $"{typeName}Service", @namespace);

        var code = @$"using System.Data;
                  using {assemblyName}.DataAccess;
                  using {candidate.ContainingNamespace};
                  using D9bolic.EntityFramework.DbContextScope.Interfaces;
                  using Microsoft.EntityFrameworkCore;

                  namespace {@namespace};    

                  {GenerateInterface(candidate, typeName)}

                  {GenerateClass(candidate, typeName, partialClasses)}";

        var fileName = $" {assemblyName}.Services.{typeName}Service.g.cs"!;
        context.AddSource(fileName, code.FormatCode());
    }

    private static string GenerateInterface(ITypeSymbol candidate, string typeName)
    {
        return @$"public partial interface I{typeName}Service
        {{
                Task<IEnumerable<{typeName}>> GetAll();

                Task<{typeName}?> GetById(int id);
        
                Task<int> Create({typeName} entity);
        
                Task Update({typeName} entity);
        
                Task Delete(int entityId);

                {GenerateInterfaceForeignMethods(candidate, typeName)}
        }}";
    }

    private static string GenerateInterfaceForeignMethods(ITypeSymbol candidate, string typeName)
    {
        return string.Join(" ", candidate
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.GetAttributes().Any(attr =>
                attr.AttributeClass.IsBaseClass("ForeignKeyAttribute", "System.ComponentModel.DataAnnotations.Schema")))
            .Select(prop =>
            {
                var foreignName = prop.Name.TrimEnd("Id");
                return $"Task<IEnumerable<{typeName}>> GetFor{foreignName}(int {prop.Name});";
            }));
    }

    private static string GenerateClass(ITypeSymbol candidate, string typeName, IEnumerable<ITypeSymbol> partialClasses)
    {
        var dependencies = partialClasses.GetDependencies();
        var methods = partialClasses.GetMethods();
        return @$"public partial class {typeName}Service : I{typeName}Service
        {{
                private readonly IRepository<{typeName}> _repository;
                private readonly IDbContextScopeFactory _dbContextScopeFactory;
            
                public {typeName}Service (IRepository<{typeName}> repository, IDbContextScopeFactory dbContextScopeFactory{dependencies.GetConstructorArguments()})
                {{
                    _repository = repository;
                    _dbContextScopeFactory = dbContextScopeFactory;
                    {dependencies.GetConstructorAssigment()}
                }}

                public async Task<IEnumerable<{typeName}>> GetAll()
                {{
                    using var scope = _dbContextScopeFactory.CreateReadOnly();
                    return await _repository.Get().ToArrayAsync();
                }}
        
                public async Task<{typeName}?> GetById(int id)
                {{
                    using var scope = _dbContextScopeFactory.CreateReadOnly();
                    return await _repository.GetById(id);
                }}

                public async Task<int> Create({typeName} entity)
                {{
                    using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
                    _repository.Add(entity);
                    await scope.SaveChangesAsync();
                    {(methods.Any(x => x.Name == "OnCreated") ? "await OnCreated(entity);" : "")}
                    return entity.Id;
                }}
        
                public async Task Update({typeName} entity)
                {{
                    using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
                    _repository.Update(entity);
                    await scope.SaveChangesAsync();
                    {(methods.Any(x => x.Name == "OnUpdated") ? "await OnUpdated(entity);" : "")}
                }}
        
                public async Task Delete(int entityId)
                {{
                    using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
                    var entity = await _repository.GetById(entityId);
                    _repository.Delete(entity);
                    await scope.SaveChangesAsync();
                    {(methods.Any(x => x.Name == "OnDeleted") ? "await OnDeleted(entity);" : "")}
                }}

                {GenerateClassForeignMethods(candidate, typeName)}
        }}";
    }

    private static string GenerateClassForeignMethods(ITypeSymbol candidate, string typeName)
    {
        return string.Join(" ", candidate
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.GetAttributes().Any(attr =>
                attr.AttributeClass.IsBaseClass("ForeignKeyAttribute", "System.ComponentModel.DataAnnotations.Schema")))
            .Select(prop =>
            {
                var foreignName = prop.Name.TrimEnd("Id");
                return @$"public async Task<IEnumerable<{typeName}>> GetFor{foreignName}(int {prop.Name})
                          {{
                                using var scope = _dbContextScopeFactory.CreateReadOnly();
                                return await _repository.Get().Where(entity => entity.{prop.Name} == {prop.Name}).ToArrayAsync();
                          }}";
            }));
    }
}