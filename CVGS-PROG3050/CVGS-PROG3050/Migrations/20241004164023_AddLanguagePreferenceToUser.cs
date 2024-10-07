using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS_PROG3050.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguagePreferenceToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanguagePreference",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguagePreference",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Address");
        }
    }
}
