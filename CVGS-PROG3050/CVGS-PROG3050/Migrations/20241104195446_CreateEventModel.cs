using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS_PROG3050.Migrations
{
    /// <inheritdoc />
    public partial class CreateEventModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Events",
                newName: "EventName");

            migrationBuilder.AddColumn<string>(
                name: "LocationType",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "EventName",
                table: "Events",
                newName: "Name");
        }
    }
}
