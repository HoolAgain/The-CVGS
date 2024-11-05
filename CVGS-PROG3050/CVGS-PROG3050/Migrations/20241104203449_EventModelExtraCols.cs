using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS_PROG3050.Migrations
{
    /// <inheritdoc />
    public partial class EventModelExtraCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EventPrice",
                table: "Events",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventPrice",
                table: "Events");
        }
    }
}
