//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace Accountool.Models;

//public partial class AccountoolContext : DbContext
//{
//    public AccountoolContext()
//    {
//    }

//    public AccountoolContext(DbContextOptions<AccountoolContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

//    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

//    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

//    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

//    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

//    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

//    public virtual DbSet<ClientProfile> ClientProfiles { get; set; }

//    public virtual DbSet<Equipment> Equipment { get; set; }

//    public virtual DbSet<Indication> Indications { get; set; }

//    public virtual DbSet<Kiosk> Kiosks { get; set; }

//    public virtual DbSet<KioskSection> KioskSections { get; set; }

//    public virtual DbSet<OrgName> OrgNames { get; set; }

//    public virtual DbSet<Organization> Organizations { get; set; }

//    public virtual DbSet<Schetchik> Schetchiks { get; set; }

//    public virtual DbSet<Town> Towns { get; set; }
    
//    public virtual DbSet<UserKiosk> UserKiosks { get; set; }
    
//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=BEROCKER\\SQLSERVER;Database=Accountool;User Id=sa;Password=Password1!;MultipleActiveResultSets=true;TrustServerCertificate=True;");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<AspNetRole>(entity =>
//        {
//            entity.Property(e => e.Name).HasMaxLength(256);
//            entity.Property(e => e.NormalizedName).HasMaxLength(256);
//        });

//        modelBuilder.Entity<AspNetRoleClaim>(entity =>
//        {
//            entity.Property(e => e.RoleId).HasMaxLength(450);

//            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
//        });

//        modelBuilder.Entity<AspNetUser>(entity =>
//        {
//            entity.Property(e => e.Email).HasMaxLength(256);
//            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
//            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
//            entity.Property(e => e.UserName).HasMaxLength(256);

//            entity.HasMany(d => d.Kiosks).WithMany(p => p.Users)
//                .UsingEntity<Dictionary<string, object>>(
//                    "UserKiosk",
//                    r => r.HasOne<Kiosk>().WithMany()
//                        .HasForeignKey("KioskId")
//                        .HasConstraintName("FK_UserKiosks_Kiosks"),
//                    l => l.HasOne<AspNetUser>().WithMany()
//                        .HasForeignKey("UserId")
//                        .HasConstraintName("FK_UserKiosks_AspNetUsers"),
//                    j =>
//                    {
//                        j.HasKey("UserId", "KioskId").HasName("PK__UserKios__B574A52DACFABF72");
//                        j.ToTable("UserKiosks");
//                    });

//            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
//                .UsingEntity<Dictionary<string, object>>(
//                    "AspNetUserRole",
//                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
//                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
//                    j =>
//                    {
//                        j.HasKey("UserId", "RoleId");
//                        j.ToTable("AspNetUserRoles");
//                    });
//        });

//        modelBuilder.Entity<AspNetUserClaim>(entity =>
//        {
//            entity.Property(e => e.UserId).HasMaxLength(450);

//            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
//        });

//        modelBuilder.Entity<AspNetUserLogin>(entity =>
//        {
//            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

//            entity.Property(e => e.LoginProvider).HasMaxLength(128);
//            entity.Property(e => e.ProviderKey).HasMaxLength(128);
//            entity.Property(e => e.UserId).HasMaxLength(450);

//            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
//        });

//        modelBuilder.Entity<AspNetUserToken>(entity =>
//        {
//            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

//            entity.Property(e => e.LoginProvider).HasMaxLength(128);
//            entity.Property(e => e.Name).HasMaxLength(128);

//            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
//        });

//        modelBuilder.Entity<ClientProfile>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__ClientPr__3214EC07FDD58F6E");

//            entity.ToTable("ClientProfile");

//            entity.Property(e => e.Address).HasMaxLength(200);
//            entity.Property(e => e.UserId).HasMaxLength(450);

//            entity.HasOne(d => d.User).WithMany(p => p.ClientProfiles)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.Cascade)
//                .HasConstraintName("FK_ClientProfile_AspNetUsers");
//        });

//        modelBuilder.Entity<Equipment>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Equipmen__3214EC07700D4081");

//            entity.Property(e => e.ModelEq).HasMaxLength(200);

//            entity.HasOne(d => d.Kiosk).WithMany(p => p.Equipment)
//                .HasForeignKey(d => d.KioskId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Equipment_Kiosk");
//        });

//        modelBuilder.Entity<Indication>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Indicati__3214EC074F02BB09");

//            entity.ToTable("Indication");

//            entity.Property(e => e.Month).HasColumnType("datetime");

//            entity.HasOne(d => d.Schetchik).WithMany(p => p.Indications)
//                .HasForeignKey(d => d.SchetchikId)
//                .HasConstraintName("FK_Indication_Schetchik");
//        });

//        modelBuilder.Entity<Kiosk>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Kiosk__3214EC0772418DFD");

//            entity.ToTable("Kiosk");

//            entity.Property(e => e.Adress).HasMaxLength(200);
//            entity.Property(e => e.Arenda).HasColumnType("datetime");
//            entity.Property(e => e.ModelKiosk).HasMaxLength(200);
//            entity.Property(e => e.Nomer).HasMaxLength(200);

//            entity.HasOne(d => d.KioskSection).WithMany(p => p.Kiosks)
//                .HasForeignKey(d => d.KioskSectionId)
//                .HasConstraintName("FK_Kiosk_KioskSection");

//            entity.HasOne(d => d.Organization).WithMany(p => p.Kiosks)
//                .HasForeignKey(d => d.OrganizationId)
//                .HasConstraintName("FK_Kiosk_Organization");

//            entity.HasOne(d => d.Town).WithMany(p => p.Kiosks)
//                .HasForeignKey(d => d.TownId)
//                .HasConstraintName("FK_Kiosk_Town");
//        });

//        modelBuilder.Entity<KioskSection>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__KioskSec__3214EC071F446306");

//            entity.ToTable("KioskSection");

//            entity.Property(e => e.AdresSection).HasMaxLength(200);
//            entity.Property(e => e.Certificate).HasMaxLength(200);
//            entity.Property(e => e.DataResh).HasColumnType("datetime");
//            entity.Property(e => e.DateArenda).HasColumnType("datetime");
//            entity.Property(e => e.Kadastr).HasMaxLength(200);
//            entity.Property(e => e.NomerKioska).HasMaxLength(200);
//            entity.Property(e => e.TypeArenda).HasMaxLength(200);
//        });

//        modelBuilder.Entity<OrgName>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__OrgName__3214EC073C8B27FC");

//            entity.ToTable("OrgName");

//            entity.Property(e => e.Name).HasMaxLength(200);
//        });

//        modelBuilder.Entity<Organization>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Organiza__3214EC07EF196410");

//            entity.ToTable("Organization");

//            entity.Property(e => e.Email).HasMaxLength(200);

//            entity.HasOne(d => d.OrgName).WithMany(p => p.Organizations)
//                .HasForeignKey(d => d.OrgNameId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Kiosk_OrgName");
//        });

//        modelBuilder.Entity<Schetchik>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Schetchi__3214EC07CC0F6E20");

//            entity.ToTable("Schetchik");

//            entity.Property(e => e.ModelSchetchika).HasMaxLength(200);
//            entity.Property(e => e.NomerSchetchika).HasMaxLength(200);
//            entity.Property(e => e.Poverka).HasColumnType("datetime");

//            entity.HasOne(d => d.Kiosk).WithMany(p => p.Schetchiks)
//                .HasForeignKey(d => d.KioskId)
//                .OnDelete(DeleteBehavior.Cascade)
//                .HasConstraintName("FK_Schetchik_Kiosk");
//        });

//        modelBuilder.Entity<Town>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Town__3214EC077070A904");

//            entity.ToTable("Town");

//            entity.Property(e => e.Name).HasMaxLength(200);
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
