using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;

namespace D9bolic.EntityFramework.DbContextScope.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Convenience method to get the <see cref="IStateManager"/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IStateManager GetStateManager(this DbContext context)
        {
            // seems to work for both frameworks
            // v2.2.6
            // v3.1.1
            return context.GetDependencies().StateManager;
        }
    }
}