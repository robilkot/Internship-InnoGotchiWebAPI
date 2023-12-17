﻿using Microsoft.EntityFrameworkCore;
using InnoGotchiWebAPI.Models;

namespace InnoGotchiWebAPI.Database;

public partial class InnoGotchiContext : DbContext
{
    public InnoGotchiContext()
    {
    }

    public InnoGotchiContext(DbContextOptions<InnoGotchiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DbPetModel> Pets { get; set; }

    public virtual DbSet<DbUserModel> Users { get; set; }

    public virtual DbSet<DbUsersPetModel> UsersPets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=InnoGotchiWeb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbPetModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pets__3214EC0702476175");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.LastDrinkTime).HasColumnType("datetime");
            entity.Property(e => e.LastEatTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OwnerId).HasMaxLength(20);
            entity.Property(e => e.Updated).HasColumnType("datetime");
        });

        modelBuilder.Entity<DbUserModel>(entity =>
        {
            entity.HasKey(e => e.Login);

            entity.Property(e => e.Login).HasMaxLength(20);
            entity.Property(e => e.Nickname).HasMaxLength(20);
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<DbUsersPetModel>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserLogin).HasMaxLength(20);

            entity.HasOne(d => d.Pet).WithMany()
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPets_PetId");

            entity.HasOne(d => d.UserLoginNavigation).WithMany()
                .HasForeignKey(d => d.UserLogin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPets_UserLogin");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
