using JobOffersFetcher.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobOffersFetcher.Infrastructure.Data;

public class JobOffersFetcherContext : DbContext
{
    public JobOffersFetcherContext(DbContextOptions<JobOffersFetcherContext> options)
        : base(options)
    {
    }

    public DbSet<Entreprise> Entreprises { get; set; }
    public DbSet<Offre> Offres { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Entreprise>(entity =>
        {
            entity.HasKey(e => e.Nom);
            
            entity.Property(e => e.Description)
                .HasColumnType("TEXT")
                .IsRequired(false);

            entity.Property(e => e.Nom)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            entity.Property(e => e.Logo)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);;

            entity.Property(e => e.Url)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);;

            entity.Property(e => e.EntrepriseAdaptee)
                .HasColumnType("bit");
        });
        
        
        builder.Entity<Offre>(entity =>
        {
            entity.Property(e => e.Id)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            entity.Property(e => e.Intitule)
                .HasColumnType("varchar")
                .HasMaxLength(255);

            entity.Property(e => e.Description)
                .HasColumnType("TEXT");

            entity.Property(e => e.DateCreation)
                .HasColumnType("datetime");

            entity.Property(e => e.DateActualisation)
                .HasColumnType("datetime");

            entity.Property(e => e.TypeContrat)
                .HasColumnType("varchar")
                .HasMaxLength(25);

            entity.Property(e => e.UrlPostulation)
                .HasColumnType("varchar")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(e => e.Provider)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            entity.HasOne(e => e.Entreprise)
                .WithMany()
                .HasForeignKey("EntrepriseId");
            entity.HasOne(e => e.LieuTravail)
                .WithMany()
                .HasForeignKey("LieuTravailId");
        });
        
        builder.Entity<LieuTravail>(entity =>
        {
            entity.Property(e => e.Libelle)
                .HasColumnType("varchar")
                .IsRequired(false)
                .HasMaxLength(255);

            entity.Property(e => e.Latitude)
                .HasColumnType("double");

            entity.Property(e => e.Longitude)
                .HasColumnType("double");

            entity.Property(e => e.CodePostal)
                .IsRequired(false)
                .HasColumnType("varchar")
                .HasMaxLength(10);

            entity.Property(e => e.Commune)
                .IsRequired(false)
                .HasColumnType("varchar")
                .HasMaxLength(255);
        });

    }
    
}