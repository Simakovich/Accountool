using System.Collections.Generic;
using D9bolic.Generator.API.CRUD.Utils;
using Microsoft.CodeAnalysis;

namespace D9bolic.Generator.API.CRUD.Providers;

public static class RepositoryGenerator
{
    public static void Generate(GeneratorExecutionContext context, IEnumerable<ITypeSymbol> candidates)
    {
        var assemblyName = context.Compilation.AssemblyName;

        var code = @$"using System.Data;
                      using D9bolic.EntityFramework.DbContextScope.Implementations;
                      using D9bolic.EntityFramework.DbContextScope.Interfaces;
                      using Microsoft.EntityFrameworkCore;
  
                  namespace {assemblyName}.DataAccess;    

                  public interface IRepository<TEntity> where TEntity : class, IEntity
                  {{
                      Task<TEntity?> GetById(int id);
                  
                      IQueryable<TEntity> Get();
                  
                      public void Add(TEntity entity);
                      
                      public void AddRange(IEnumerable<TEntity> entities);
                      
                      public void Delete(TEntity entity);
                  
                      public void Update(TEntity entity);
                      
                      void DeleteRange(IEnumerable<TEntity> records);
                  }}

                                    /// <inheritdoc />
                            public class GenericSqlRepository<TEntity> : IRepository<TEntity>
                                where TEntity : class, IEntity
                            {{
                                private readonly IAmbientDbContextLocator _ambientDbContextLocator;
                                private readonly IDbContextScopeFactory _contextScopeFactory;
                                private readonly IDbContextScopeContainer _dbContextScopeContainer;

                                /// <summary>
                                /// Initializes a new instance of the <see cref=""Repository{{TEntity}}"" /> class.
                                /// </summary>
                                /// <param name=""ambientDbContextLocator"">database context locator</param>
                                /// <param name=""contextScopeFactory""></param>
                                /// <param name=""dbContextScopeContainer"">Db context scope container which can detect current db scope transaction scope</param>
                                public GenericSqlRepository(IAmbientDbContextLocator ambientDbContextLocator, IDbContextScopeFactory contextScopeFactory,
                                    IDbContextScopeContainer dbContextScopeContainer)
                                {{
                                    if (ambientDbContextLocator == null)
                                    {{
                                        throw new ArgumentNullException(nameof(ambientDbContextLocator));
                                    }}

                                    _ambientDbContextLocator = ambientDbContextLocator;
                                    _dbContextScopeContainer = dbContextScopeContainer;
                                    _contextScopeFactory = contextScopeFactory;
                                }}

                                /// <inheritdoc />
                                public IQueryable<TEntity> Get()
                                {{
                                    DbScopeOption option;
                                    var context = GetContextWithCurrentDbScopeOptionIfPossible(DbScopeOption.TransactionalReadOnly, out option);
                                    return option == DbScopeOption.TransactionalReadOnly
                                        ? context.Set<TEntity>().AsNoTracking()
                                        : context.Set<TEntity>();
                                }}

                                /// <inheritdoc />
                                public async Task<TEntity?> GetById(int id)
                                {{
                                    var context = GetContextIfPossible(DbScopeOption.TransactionalReadOnly);
                                    return await context.Set<TEntity>().FindAsync(id);
                                }}

                                /// <inheritdoc />
                                public void Add(TEntity entity)
                                {{
                                    using var dbContextScope = _contextScopeFactory.Create();
                                    var context = GetContextIfPossible(DbScopeOption.Transactional);
                                    context.Set<TEntity>().Add(entity);
                                    dbContextScope.SaveChangesWithoutCommitWithinParentScopes();
                                }}

                                public void AddRange(IEnumerable<TEntity> entities)
                                {{
                                    using var dbContextScope = _contextScopeFactory.Create();
                                    var context = GetContextIfPossible(DbScopeOption.Transactional);
                                    context.Set<TEntity>().AddRange(entities);
                                    dbContextScope.SaveChangesWithoutCommitWithinParentScopes();
                                }}

                                /// <inheritdoc />
                                public void Update(TEntity entity)
                                {{
                                    using var dbContextScope = _contextScopeFactory.Create();
                                    var context = GetContextIfPossible(DbScopeOption.Transactional);
                                    context.Entry(entity).State = EntityState.Modified;
                                    dbContextScope.SaveChangesWithoutCommitWithinParentScopes();
                                }}

                                public void DeleteRange(IEnumerable<TEntity> records)
                                {{
                                    using var dbContextScope = _contextScopeFactory.Create();
                                    var context = GetContextIfPossible(DbScopeOption.Transactional);
                                    var set = context.Set<TEntity>();
                                    set.RemoveRange(records);
                                    dbContextScope.SaveChangesWithoutCommitWithinParentScopes();
                                }}

                                /// <inheritdoc />
                                public void Delete(TEntity entity)
                                {{
                                    using var dbContextScope = _contextScopeFactory.Create();
                                    var context = GetContextIfPossible(DbScopeOption.Transactional);
                                    var set = context.Set<TEntity>();
                                    if (context.Entry(entity).State == EntityState.Detached)
                                    {{
                                        set.Attach(entity);
                                    }}

                                    set.Remove(entity);
                                    dbContextScope.SaveChangesWithoutCommitWithinParentScopes();
                                }}
                                

                                /// <summary>
                                /// Gets context with this option if possible
                                /// </summary>
                                /// <param name=""expectedOption"">Expected option</param>
                                /// <returns>Db context</returns>
                                protected DbContext GetContextIfPossible(DbScopeOption expectedOption)
                                {{
                                    DbScopeOption option;
                                    return GetContextWithCurrentDbScopeOptionIfPossible(expectedOption, out option);
                                }}

                                /// <summary>
                                /// Execute operation on entity for each entity in set.
                                /// At the start of entity sequence EF Change tracker will be disabled and enabled at the end again
                                /// </summary>
                                /// <param name=""entities"">sequence of entities</param>
                                /// <param name=""entityOperation"">operation on entity</param>
                                private void ForEachEntityWithOneChangeTrackerCall(
                                    IEnumerable<TEntity> entities,
                                    Action<TEntity> entityOperation)
                                {{
                                    WithOneChangeTrackerCall(() =>
                                    {{
                                        foreach (var entity in entities)
                                        {{
                                            entityOperation(entity);
                                        }}
                                    }});
                                }}

                                /// <summary>
                                /// Perform action with one call of change tracker
                                /// </summary>
                                /// <param name=""operation"">context operation</param>
                                private void WithOneChangeTrackerCall(Action operation)
                                {{
                                    var context = GetContextIfPossible(DbScopeOption.Transactional);
                                    try
                                    {{
                                        context.ChangeTracker.AutoDetectChangesEnabled = false;
                                        operation.Invoke();
                                    }}
                                    finally
                                    {{
                                        context.ChangeTracker.AutoDetectChangesEnabled = true;
                                    }}
                                }}

                                private DbContext GetContextWithCurrentDbScopeOptionIfPossible(DbScopeOption expectedOption,
                                    out DbScopeOption option)
                                {{
                                    var context = _ambientDbContextLocator.Get<ApplicationsDbContext>();
                                    if (context == null)
                                    {{
                                        throw new InvalidOperationException(
                                            $""No ambient DbContext of type {{typeof(ApplicationsDbContext)}}. Try to check transactions on your services."");
                                    }}

                                    option = _dbContextScopeContainer.GetDbScopeOption(context);
                                    if ((byte)option < (byte)expectedOption)
                                    {{
                                        throw new InvalidOperationException(
                                            $""Scope of operation is {{option}} and it's less than expected scope {{expectedOption}}."");
                                    }}

                                    return context;
                                }}
                            }}";

        var fileName = $" {assemblyName}.DataAccess.Repository.g.cs"!;
        context.AddSource(fileName, code.FormatCode());
    }
}