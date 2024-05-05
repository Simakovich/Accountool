using Microsoft.EntityFrameworkCore;

namespace Accountool.DataAccess;
public class DbContextFactory : D9bolic.EntityFramework.DbContextScope.Interfaces.IDbContextFactory
{
    private readonly Action<DbContextOptionsBuilder> _options;
    public DbContextFactory(Action<DbContextOptionsBuilder> options)
    {
        _options = options;
    }

    public TDbContext CreateDbContext<TDbContext>()
        where TDbContext : DbContext
    {
        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
        _options(optionsBuilder);
        return ((TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options));
    }
}