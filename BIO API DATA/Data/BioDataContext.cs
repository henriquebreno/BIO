using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BIO_API_DATA.Data;

public partial class BioDataContext : DbContext
{
    public BioDataContext()
    {
    }

    public BioDataContext(DbContextOptions<BioDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<GasMeterCustomerRelation> GasMeterCustomerRelations { get; set; }

    public virtual DbSet<GasMeterMeasurement> GasMeterMeasurements { get; set; }

    public virtual DbSet<GasMeteringPoint> GasMeteringPoints { get; set; }

    public virtual DbSet<Observation> Observations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=BIO-data;Integrated Security=true;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BuildingFloor).HasMaxLength(255);
            entity.Property(e => e.BuildingNumber).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Country).HasMaxLength(255);
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.CustomerNumber).HasMaxLength(255);
            entity.Property(e => e.EffectiveEndTimeUtc).HasColumnType("datetime");
            entity.Property(e => e.EffectiveStartTimeUtc).HasColumnType("datetime");
            entity.Property(e => e.MunicipalityCode).HasMaxLength(255);
            entity.Property(e => e.PostalCode).HasMaxLength(255);
            entity.Property(e => e.RoomIdentification).HasMaxLength(255);
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.StreetName).HasMaxLength(255);
            entity.Property(e => e.VatIdentification).HasMaxLength(255);
        });

        modelBuilder.Entity<GasMeterCustomerRelation>(entity =>
        {
            entity.ToTable("GasMeterCustomerRelation");

            entity.Property(e => e.EffectiveEndTimeUtc).HasColumnType("datetime");
            entity.Property(e => e.EffectiveStartTimeUtc).HasColumnType("datetime");
            entity.Property(e => e.Source).HasMaxLength(255);

            entity.HasOne(d => d.Customer).WithMany(p => p.GasMeterCustomerRelations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_GasMeterCustomerRelation_Customer");

            entity.HasOne(d => d.GasMeteringPoint).WithMany(p => p.GasMeterCustomerRelations)
                .HasForeignKey(d => d.GasMeteringPointId)
                .HasConstraintName("FK_GasMeterCustomerRelation_GasMeteringPoint");
        });

        modelBuilder.Entity<GasMeterMeasurement>(entity =>
        {
            entity.HasIndex(e => e.MeteringPointIdentification, "IX_GasMeterMeasurements").IsUnique();

            entity.Property(e => e.End).HasColumnType("datetime");
            entity.Property(e => e.Resolution).HasMaxLength(255);
            entity.Property(e => e.Start).HasColumnType("datetime");
            entity.Property(e => e.Unit).HasMaxLength(255);

            entity.HasOne(d => d.MeteringPointIdentificationNavigation).WithOne(p => p.GasMeterMeasurement)
                .HasForeignKey<GasMeterMeasurement>(d => d.MeteringPointIdentification)
                .HasConstraintName("FK_GasMeterMeasurements_GasMeteringPoint");
        });

        modelBuilder.Entity<GasMeteringPoint>(entity =>
        {
            entity.ToTable("GasMeteringPoint");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EffectiveEndTimeUtc).HasColumnType("datetime");
            entity.Property(e => e.EffectiveStartTimeUtc).HasColumnType("datetime");
            entity.Property(e => e.InstalationPostalCode).HasMaxLength(255);
            entity.Property(e => e.InstallationBuildingFloor).HasMaxLength(255);
            entity.Property(e => e.InstallationBuildingNumber).HasMaxLength(255);
            entity.Property(e => e.InstallationCity).HasMaxLength(255);
            entity.Property(e => e.InstallationCountry).HasMaxLength(255);
            entity.Property(e => e.InstallationMunicipalityCode).HasMaxLength(255);
            entity.Property(e => e.InstallationRoomIdentification).HasMaxLength(255);
            entity.Property(e => e.InstallationStreetName).HasMaxLength(255);
            entity.Property(e => e.PriceAreaCode).HasMaxLength(255);
            entity.Property(e => e.Source).HasMaxLength(255);
        });

        modelBuilder.Entity<Observation>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_Observations").IsUnique();

            entity.Property(e => e.Quality).HasMaxLength(255);
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.GasMeterMeasurement).WithMany(p => p.Observations)
                .HasForeignKey(d => d.GasMeterMeasurementId)
                .HasConstraintName("FK_Observations_GasMeterMeasurements");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
