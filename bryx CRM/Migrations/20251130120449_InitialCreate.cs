using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bryx_CRM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Supplier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsDefective = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "IsDefective", "Name", "PurchasePrice", "SalePrice", "Status", "Supplier", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Наушники", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Sony WH-1000XM5", 25000m, 29999m, "В наличии", "ООО Техносервис", null },
                    { 2, "Наушники", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Apple AirPods Pro", 20000m, 24990m, "В наличии", "ИП Иванов", null },
                    { 3, "Клавиатуры", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Logitech MX Keys", 9000m, 11990m, "В наличии", "ООО Техносервис", null },
                    { 4, "Мышки", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Logitech MX Master 3", 7000m, 8990m, "В наличии", "ИП Петров", null },
                    { 5, "Микрофоны", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Blue Yeti", 12000m, 14990m, "Продано", "ООО Техносервис", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Status",
                table: "Products",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
