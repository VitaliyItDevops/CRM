using bryx_CRM.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bryx_CRM.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройки для таблицы Products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Status);

                entity.Property(e => e.PurchasePrice)
                    .HasPrecision(18, 2);

                entity.Property(e => e.SalePrice)
                    .HasPrecision(18, 2);
            });

            // Настройки для таблицы Categories
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");

                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Настройки для таблицы Subcategories
            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.ToTable("Subcategories");

                entity.HasIndex(e => new { e.CategoryName, e.Name }).IsUnique();
                entity.HasIndex(e => e.CategoryName);
            });

            // Настройки для таблицы Suppliers
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Suppliers");

                entity.HasIndex(e => e.Name);
            });

            // Настройки для таблицы Sales
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sales");

                entity.Property(e => e.TotalAmount)
                    .HasPrecision(18, 2);

                // Настройка связи один-ко-многим (одна продажа - много товаров)
                entity.HasMany(s => s.Products)
                    .WithOne(p => p.Sale)
                    .HasForeignKey(p => p.SaleId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Настройки для таблицы Purchases
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchases");

                entity.Property(e => e.TotalPriceUSD)
                    .HasPrecision(18, 2);

                entity.Property(e => e.ExchangeRate)
                    .HasPrecision(18, 4);

                entity.Property(e => e.TotalPriceUAH)
                    .HasPrecision(18, 2);

                // Настройка связи один-ко-многим (одна покупка - много товаров)
                entity.HasMany(p => p.Products)
                    .WithOne(p => p.Purchase)
                    .HasForeignKey(p => p.PurchaseId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Добавляем тестовые категории
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Наушники",
                    Description = "Проводные и беспроводные наушники",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Category
                {
                    Id = 2,
                    Name = "Клавиатуры",
                    Description = "Механические и мембранные клавиатуры",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Category
                {
                    Id = 3,
                    Name = "Мышки",
                    Description = "Игровые и офисные мышки",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Category
                {
                    Id = 4,
                    Name = "Микрофоны",
                    Description = "USB и XLR микрофоны для стриминга",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Category
                {
                    Id = 5,
                    Name = "Вебкамеры",
                    Description = "Веб-камеры для видеозвонков и стриминга",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Добавляем тестовые подкатегории
            modelBuilder.Entity<Subcategory>().HasData(
                new Subcategory
                {
                    Id = 1,
                    CategoryName = "Наушники",
                    Name = "Беспроводные",
                    Description = "Bluetooth наушники",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Subcategory
                {
                    Id = 2,
                    CategoryName = "Наушники",
                    Name = "Проводные",
                    Description = "Наушники с кабелем",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Subcategory
                {
                    Id = 3,
                    CategoryName = "Клавиатуры",
                    Name = "Механические",
                    Description = "Клавиатуры с механическими переключателями",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Subcategory
                {
                    Id = 4,
                    CategoryName = "Клавиатуры",
                    Name = "Мембранные",
                    Description = "Клавиатуры с мембранными переключателями",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Subcategory
                {
                    Id = 5,
                    CategoryName = "Мышки",
                    Name = "Игровые",
                    Description = "Мышки для геймеров",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Subcategory
                {
                    Id = 6,
                    CategoryName = "Мышки",
                    Name = "Офисные",
                    Description = "Мышки для работы",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Добавляем тестовых поставщиков
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    Id = 1,
                    Name = "ООО Техносервис",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Supplier
                {
                    Id = 2,
                    Name = "ИП Иванов",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Supplier
                {
                    Id = 3,
                    Name = "ИП Петров",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Добавляем тестовые товары
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Category = "Наушники",
                    Name = "Sony WH-1000XM5",
                    PurchasePrice = 25000,
                    SalePrice = 29999,
                    Status = "В наличии",
                    Supplier = "ООО Техносервис",
                    IsDefective = false,
                    IsFavorite = false,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 2,
                    Category = "Наушники",
                    Name = "Apple AirPods Pro",
                    PurchasePrice = 20000,
                    SalePrice = 24990,
                    Status = "В наличии",
                    Supplier = "ИП Иванов",
                    IsDefective = false,
                    IsFavorite = false,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 3,
                    Category = "Клавиатуры",
                    Name = "Logitech MX Keys",
                    PurchasePrice = 9000,
                    SalePrice = 11990,
                    Status = "В наличии",
                    Supplier = "ООО Техносервис",
                    IsDefective = false,
                    IsFavorite = false,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 4,
                    Category = "Мышки",
                    Name = "Logitech MX Master 3",
                    PurchasePrice = 7000,
                    SalePrice = 8990,
                    Status = "В наличии",
                    Supplier = "ИП Петров",
                    IsDefective = false,
                    IsFavorite = false,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 5,
                    Category = "Микрофоны",
                    Name = "Blue Yeti",
                    PurchasePrice = 12000,
                    SalePrice = 14990,
                    Status = "Продано",
                    Supplier = "ООО Техносервис",
                    IsDefective = false,
                    IsFavorite = false,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}