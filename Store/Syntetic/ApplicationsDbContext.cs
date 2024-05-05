using System.Data;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.DataAccess;
public class ApplicationsDbContext : DbContext
{
    public ApplicationsDbContext(DbContextOptions<ApplicationsDbContext> options) : base(options)
    {
    }

    public DbSet<Equipment> Equipment { get; set; } = null !;
    public DbSet<OrgName> OrgNames { get; set; } = null !;
    public DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null !;
    public DbSet<ClientProfile> ClientProfiles { get; set; } = null !;
    public DbSet<KioskSection> KioskSections { get; set; } = null !;
    public DbSet<Kiosk> Kiosks { get; set; } = null !;
    public DbSet<Indication> Indications { get; set; } = null !;
    public DbSet<Schetchik> Schetchiks { get; set; } = null !;
    public DbSet<Organization> Organizations { get; set; } = null !;
    public DbSet<Town> Towns { get; set; } = null !;
}