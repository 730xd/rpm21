using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace rpm21.Models;

public partial class AtelierDbContext : DbContext
{
    public AtelierDbContext()
    {
    }

    public AtelierDbContext(DbContextOptions<AtelierDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atelier> Ateliers { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServicePrice> ServicePrices { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=AtelierDB;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Atelier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ateliers__3214EC07702674E6");

            entity.HasIndex(e => e.Number, "UQ__Ateliers__78A1A19DF3F0202C").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PhotoPath).HasMaxLength(500);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3214EC07FC27F3E8");

            entity.HasIndex(e => e.Code, "UQ__Services__A25C5AA737CA9E4E").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ServicePrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceP__3214EC071AEAEAF2");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Atelier).WithMany(p => p.ServicePrices)
                .HasForeignKey(d => d.AtelierId)
                .HasConstraintName("FK__ServicePr__Ateli__4222D4EF");

            entity.HasOne(d => d.Service).WithMany(p => p.ServicePrices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServicePr__Servi__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07F22DDA9F");

            entity.HasIndex(e => e.Login, "UQ__Users__5E55825BBA08B2A1").IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
