using D9bolic.EntityFramework.DbContextScope.Implementations;
using Microsoft.EntityFrameworkCore;

namespace D9bolic.EntityFramework.DbContextScope.Interfaces
{
    /// <summary>
    /// Container which helps identify scope of current db context
    /// </summary>
    public interface IDbContextScopeContainer
    {
        /// <summary>
        /// Returns scope of current context
        /// </summary>
        /// <typeparam name="TContext">Type of db context</typeparam>
        /// <param name="currentContext">Context for identifying</param>
        /// <returns>Scope option</returns>
        DbScopeOption GetDbScopeOption<TContext>(TContext currentContext)
            where TContext : DbContext;
    }
}