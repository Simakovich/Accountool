using Accountool.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Accountool.Data
{
    //public partial class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string, AspNetUserClaim<string>,
    //    AspNetUserRoles<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    public partial class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<ClientProfile> ClientProfiles { get; set; }

        public virtual DbSet<Equipment> Equipment { get; set; }

        public virtual DbSet<Indication> Indications { get; set; }

        public virtual DbSet<Place> Places { get; set; }

        public virtual DbSet<PlaceSection> PlaceSections { get; set; }

        public virtual DbSet<Contract> Contracts { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Schetchik> Schetchiks { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        public virtual DbSet<UserPlace> UserPlaces { get; set; }
        
        public virtual DbSet<MeasureType> MeasureTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=BEROCKER\\SQLSERVER;Database=Accountool;User Id=sa;Password=Password1!;MultipleActiveResultSets=true;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientProfile>(entity =>
            {
                entity.HasKey(up => new { up.UserId, up.PlaceId }).HasName("PK__ClientProfile__3214EC07700D5062"); ;

                entity.HasOne<IdentityUser>()
                    .WithMany()
                    .HasForeignKey(cp => cp.UserId);
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Equipmen__3214EC07700D4081");

                entity.Property(e => e.ModelEq).HasMaxLength(200);

                entity.HasOne(d => d.Place).WithMany(p => p.Equipments)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Equipment_Place");
            });

            modelBuilder.Entity<Indication>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Indicati__3214EC074F02BB09");

                entity.ToTable("Indication");

                entity.Property(e => e.Month).HasColumnType("datetime");

                entity.HasOne(d => d.Schetchik).WithMany(p => p.Indications)
                    .HasForeignKey(d => d.SchetchikId)
                    .HasConstraintName("FK_Indication_Schetchik");
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Place__3214EC0772418DFD");

                entity.ToTable("Place");

                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Arenda).HasColumnType("datetime");
                entity.Property(e => e.ModelPlace).HasMaxLength(200);
                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.PlaceSection).WithMany(p => p.Places)
                    .HasForeignKey(d => d.PlaceSectionId)
                    .HasConstraintName("FK_Place_PlaceSection");

                entity.HasOne(d => d.Town).WithMany(p => p.Places)
                    .HasForeignKey(d => d.TownId)
                    .HasConstraintName("FK_Place_Town");
            });

            modelBuilder.Entity<PlaceSection>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__PlaceSec__3214EC071F446306");

                entity.ToTable("PlaceSection");

                entity.Property(e => e.AdresSection).HasMaxLength(200);
                entity.Property(e => e.Certificate).HasMaxLength(200);
                entity.Property(e => e.DataResh).HasColumnType("datetime");
                entity.Property(e => e.DateArenda).HasColumnType("datetime");
                entity.Property(e => e.Kadastr).HasMaxLength(200);
                entity.Property(e => e.PlaceName).HasMaxLength(200);
                entity.Property(e => e.TypeArenda).HasMaxLength(200);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Organiza__3214EC073C8B27FC");

                entity.ToTable("Organization");

                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(200);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Contract__3214EC07EF196410");

                entity.ToTable("Contract");

                entity.HasOne(d => d.Organization).WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_Organization");

                entity.HasOne(d => d.Place).WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_Place");
            });

            modelBuilder.Entity<Schetchik>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Schetchi__3214EC07CC0F6E20");

                entity.ToTable("Schetchik");

                entity.Property(e => e.ModelSchetchika).HasMaxLength(200);
                entity.Property(e => e.NomerSchetchika).HasMaxLength(200);
                entity.Property(e => e.Poverka).HasColumnType("datetime");

                entity.HasOne(d => d.Place).WithMany(p => p.Schetchiks)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Schetchik_Place");
                entity.HasOne(d => d.MeasureType).WithMany(p => p.Schetchiks)
                    .HasForeignKey(d => d.MeasureTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Schetchik_MeasureType");
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Town__3214EC077070A904");

                entity.ToTable("Town");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<MeasureType>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MeasureType__3214EC088080A803");

                entity.ToTable("MeasureType");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<UserPlace>()
                .HasKey(u => new { u.UserId, u.PlaceId });

            modelBuilder.Entity<UserPlace>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserPlace>()
                .HasOne(p => p.Place)
                .WithMany()
                .HasForeignKey(p => p.PlaceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            OnModelCreatingPartial(modelBuilder);base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    } 
}
