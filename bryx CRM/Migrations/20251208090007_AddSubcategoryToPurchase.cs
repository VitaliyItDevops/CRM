using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bryx_CRM.Migrations
{
    /// <inheritdoc />
    public partial class AddSubcategoryToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subcategory",
                table: "Purchases",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subcategory",
                table: "Purchases");
        }
    }
}
