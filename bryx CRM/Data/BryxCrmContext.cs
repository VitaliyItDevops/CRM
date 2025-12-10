using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using bryx_CRM.Data.Models;

namespace bryx_CRM.Data;

public partial class BryxCrmContext : DbContext
{
    public BryxCrmContext(DbContextOptions<BryxCrmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Categories_Name").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasIndex(e => e.Category, "IX_Expenses_Category");

            entity.HasIndex(e => e.ExpenseDate, "IX_Expenses_ExpenseDate");

            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<ExpenseCategory>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_ExpenseCategories_Name").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.Category, "IX_Products_Category");

            entity.HasIndex(e => e.PurchaseId, "IX_Products_PurchaseId");

            entity.HasIndex(e => e.SaleId, "IX_Products_SaleId");

            entity.HasIndex(e => e.ServiceId, "IX_Products_ServiceId");

            entity.HasIndex(e => e.Status, "IX_Products_Status");

            entity.Property(e => e.AdditionalService).HasMaxLength(300);
            entity.Property(e => e.Buyer).HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(100);
            entity.Property(e => e.ExchangeRate).HasPrecision(18, 4);
            entity.Property(e => e.IsFavorite).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PlannedPrice).HasPrecision(18, 2);
            entity.Property(e => e.PriceInUSD)
                .HasPrecision(18, 2)
                .HasColumnName("PriceInUSD");
            entity.Property(e => e.PurchasePrice).HasPrecision(18, 2);
            entity.Property(e => e.SalePrice).HasPrecision(18, 2);
            entity.Property(e => e.SoldFor).HasMaxLength(200);
            entity.Property(e => e.SoldThrough).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Subcategory).HasMaxLength(100);
            entity.Property(e => e.Supplier).HasMaxLength(200);
            entity.Property(e => e.TTN)
                .HasMaxLength(100)
                .HasColumnName("TTN");

            entity.HasOne(d => d.Purchase).WithMany(p => p.Products)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Sale).WithMany(p => p.Products)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Service).WithMany(p => p.Products).HasForeignKey(d => d.ServiceId);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.ExchangeRate).HasPrecision(18, 4);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Subcategory).HasMaxLength(100);
            entity.Property(e => e.Supplier).HasMaxLength(200);
            entity.Property(e => e.TotalPriceUAH)
                .HasPrecision(18, 2)
                .HasColumnName("TotalPriceUAH");
            entity.Property(e => e.TotalPriceUSD)
                .HasPrecision(18, 2)
                .HasColumnName("TotalPriceUSD");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.Property(e => e.AdditionalService).HasMaxLength(300);
            entity.Property(e => e.Buyer).HasMaxLength(200);
            entity.Property(e => e.SoldThrough).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.TTN)
                .HasMaxLength(100)
                .HasColumnName("TTN");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasIndex(e => e.Category, "IX_Services_Category");

            entity.HasIndex(e => e.Name, "IX_Services_Name");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Subcategory).HasMaxLength(100);
        });

        modelBuilder.Entity<ServiceCategory>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_ServiceCategories_Name").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasIndex(e => e.CategoryName, "IX_Subcategories_CategoryName");

            entity.HasIndex(e => new { e.CategoryName, e.Name }, "IX_Subcategories_CategoryName_Name").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Suppliers_Name");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
