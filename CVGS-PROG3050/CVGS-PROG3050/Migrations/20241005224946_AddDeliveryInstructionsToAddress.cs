using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS_PROG3050.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryInstructionsToAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Address",
                newName: "StreetAddress");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryInstructions",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryInstructions",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "Address",
                newName: "Street");
        }
    }
}
