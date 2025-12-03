# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is **bryx CRM** - a warehouse and inventory management system built with ASP.NET Core Blazor Server and PostgreSQL. The application is in Russian and manages products, categories, sales, purchases, and warehouse movements.

**Technology Stack:**
- .NET 10.0 (Blazor Server with Interactive Server Components)
- Entity Framework Core 10.0
- PostgreSQL (via Npgsql.EntityFrameworkCore.PostgreSQL 10.0)
- Bootstrap for UI styling

## Database Architecture

### Connection String
Located in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=bryx_crm;Username=postgres;Password=..."
}
```

### DbContext Pattern
The application uses `IDbContextFactory<ApplicationDbContext>` with **pooled DbContext** for optimal performance in Blazor Server:
```csharp
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

**Important:** Always use `await DbContextFactory.CreateDbContextAsync()` within `using` statements. Never inject `ApplicationDbContext` directly.

### Data Models

**Product** (`Data/Models/Product.cs`):
- Primary entity for inventory items
- Fields: Category, Name, PurchasePrice, SalePrice, Status, Supplier, IsDefective, IsFavorite
- Status values: "В наличии" (In stock), "Продано" (Sold), "Ожидается" (Expected), "Отправлено" (Shipped)
- Decimal precision: 18,2 for prices
- Indexed fields: Category, Status

**Category** (`Data/Models/Category.cs`):
- Warehouse organization structure
- Fields: Name (unique index), Description
- Seeded with test data: Наушники, Клавиатуры, Мышки, Микрофоны, Вебкамеры

## Component Architecture

### Page Structure
- `Components/Pages/Home.razor` - Landing page (placeholder)
- `Components/Pages/Warehouse.razor` - Main warehouse management interface
- `Components/Pages/Sales.razor` - Sales page (placeholder)
- `Components/Pages/Purchase.razor` - Purchase page (placeholder)
- `Components/Pages/Movement.razor` - Movement tracking page (placeholder)
- `Components/Pages/NotFound.razor` - 404 handler

### Warehouse Page (`/warehouse`)
This is the most complex component with full CRUD functionality:

**Features:**
- Category grid view with product counts
- Drill-down to product list per category
- Advanced filtering: search by name, filter by supplier, sort options
- Favorites system with star toggle
- Defective product marking
- Product summary view with aggregated data across statuses
- Modal for adding new categories

**Render Mode:** `@rendermode InteractiveServer`

**Key Methods:**
- `LoadCategories()` - Async load from Categories table
- `SelectCategory()` - Loads products for selected category (Status == "В наличии")
- `GenerateProductSummary()` - Aggregates products by name across all statuses
- `ToggleFavorite()`, `ToggleDefective()` - Update single properties using `context.Entry()`

**Important Pattern:**
```csharp
// Always attach and mark specific properties as modified
context.Products.Attach(product);
context.Entry(product).Property(p => p.IsFavorite).IsModified = true;
context.Entry(product).Property(p => p.UpdatedAt).IsModified = true;
await context.SaveChangesAsync();
```

### Navigation
`Components/Layout/NavMenu.razor` defines the main navigation:
- Главная (Home) - /
- Склад (Warehouse) - /warehouse
- Продажи (Sales) - /sales
- Закупка (Purchase) - /purchase
- Движение (Movement) - /movement

## Development Commands

### Build the project
```bash
dotnet build "bryx CRM\bryx CRM.csproj"
```

### Run the application
```bash
dotnet run --project "bryx CRM\bryx CRM.csproj"
```

### Entity Framework Migrations

**Add a new migration:**
```bash
dotnet ef migrations add MigrationName --project "bryx CRM\bryx CRM.csproj"
```

**Update database:**
```bash
dotnet ef database update --project "bryx CRM\bryx CRM.csproj"
```

**Remove last migration:**
```bash
dotnet ef migrations remove --project "bryx CRM\bryx CRM.csproj"
```

### Useful Commands
```bash
# List all migrations
dotnet ef migrations list --project "bryx CRM\bryx CRM.csproj"

# Generate SQL script for migration
dotnet ef migrations script --project "bryx CRM\bryx CRM.csproj"

# Drop database (careful!)
dotnet ef database drop --project "bryx CRM\bryx CRM.csproj"
```

## Project Structure Notes

- **RootNamespace:** `bryx_CRM` (uses underscore, not space)
- **AssemblyName:** Spaces replaced with underscores automatically
- **Status Code Pages:** 404 redirects to `/not-found`
- **Static Assets:** `app.MapStaticAssets()` used instead of `UseStaticFiles()`
- **Antiforgery:** Enabled globally via `app.UseAntiforgery()`

## Key Configuration

**Program.cs highlights:**
- Razor Components with Interactive Server render mode
- Exception handling: `/Error` page in production
- HSTS enabled in production
- Status code pages with re-execution to `/not-found`

## Language & Localization

- **Primary Language:** Russian (UI text, categories, status values)
- **Database Content:** Russian (product names, categories, suppliers)
- **Code Comments:** Mix of Russian and English

When modifying UI text or adding features, maintain consistency with Russian language throughout the application.

## Database Seed Data

The `ApplicationDbContext.OnModelCreating` method seeds:
- 5 categories (tech peripherals)
- 5 sample products with realistic pricing

This seed data is helpful for testing but should be removed or modified for production deployment.
