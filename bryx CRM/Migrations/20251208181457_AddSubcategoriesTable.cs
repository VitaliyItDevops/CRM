using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bryx_CRM.Migrations
{
    /// <inheritdoc />
    public partial class AddSubcategoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "Id", "CategoryName", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Наушники", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bluetooth наушники", "Беспроводные", null },
                    { 2, "Наушники", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Наушники с кабелем", "Проводные", null },
                    { 3, "Клавиатуры", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Клавиатуры с механическими переключателями", "Механические", null },
                    { 4, "Клавиатуры", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Клавиатуры с мембранными переключателями", "Мембранные", null },
                    { 5, "Мышки", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Мышки для геймеров", "Игровые", null },
                    { 6, "Мышки", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Мышки для работы", "Офисные", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryName",
                table: "Subcategories",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryName_Name",
                table: "Subcategories",
                columns: new[] { "CategoryName", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subcategories");
        }
    }
}
