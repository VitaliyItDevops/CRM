using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bryx_CRM.Migrations
{
    /// <inheritdoc />
    public partial class AddSubcategoryExchangeRateAndPriceInUSD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "Products",
                type: "numeric(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceInUSD",
                table: "Products",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subcategory",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ExchangeRate", "PriceInUSD", "Subcategory" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ExchangeRate", "PriceInUSD", "Subcategory" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ExchangeRate", "PriceInUSD", "Subcategory" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ExchangeRate", "PriceInUSD", "Subcategory" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ExchangeRate", "PriceInUSD", "Subcategory" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PriceInUSD",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Subcategory",
                table: "Products");
        }
    }
}
