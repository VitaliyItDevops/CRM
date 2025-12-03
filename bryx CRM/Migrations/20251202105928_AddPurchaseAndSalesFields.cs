using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bryx_CRM.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseAndSalesFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalService",
                table: "Products",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Buyer",
                table: "Products",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoldFor",
                table: "Products",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoldThrough",
                table: "Products",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TTN",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AdditionalService", "Buyer", "SoldFor", "SoldThrough", "TTN" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AdditionalService", "Buyer", "SoldFor", "SoldThrough", "TTN" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AdditionalService", "Buyer", "SoldFor", "SoldThrough", "TTN" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AdditionalService", "Buyer", "SoldFor", "SoldThrough", "TTN" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AdditionalService", "Buyer", "SoldFor", "SoldThrough", "TTN" },
                values: new object[] { null, null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalService",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Buyer",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SoldFor",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SoldThrough",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TTN",
                table: "Products");
        }
    }
}
