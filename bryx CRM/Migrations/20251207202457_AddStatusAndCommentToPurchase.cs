using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bryx_CRM.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusAndCommentToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Purchases",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Purchases",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Purchases");
        }
    }
}
