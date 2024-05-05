using Microsoft.Extensions.DependencyInjection.Extensions;
using D9bolic.EntityFramework.DbContextScope.Implementations;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using Accountool.Services;
using Accountool.DataAccess;

namespace Accountool.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeneratedCRUD(this IServiceCollection services, Action<DbContextOptionsBuilder> contextOptionsAction)
    {
        services.AddDbContext<ApplicationsDbContext>(contextOptionsAction);
        services.AddSingleton<D9bolic.EntityFramework.DbContextScope.Interfaces.IDbContextFactory, DbContextFactory>();
        services.AddSingleton<Action<DbContextOptionsBuilder>>(contextOptionsAction);
        services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
        services.AddSingleton<DbContextScopeFactory>();
        services.AddSingleton<DbContextScopeFactoryDispatcher>();
        services.AddSingleton<IDbContextScopeFactory>(x => x.GetService<DbContextScopeFactoryDispatcher>());
        services.AddSingleton<IDbContextScopeContainer>(x => x.GetService<DbContextScopeFactoryDispatcher>());
        services.AddScoped(typeof(IRepository<>), typeof(GenericSqlRepository<>));
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IOrgNameService, OrgNameService>();
        services.AddScoped<IAspNetRoleClaimService, AspNetRoleClaimService>();
        services.AddScoped<IClientProfileService, ClientProfileService>();
        services.AddScoped<IKioskSectionService, KioskSectionService>();
        services.AddScoped<IKioskService, KioskService>();
        services.AddScoped<IIndicationService, IndicationService>();
        services.AddScoped<ISchetchikService, SchetchikService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<ITownService, TownService>();
        return services;
    }
}