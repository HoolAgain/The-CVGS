using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS_PROG3050.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentIdToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RandomReview",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserPaymentPaymentId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserPaymentPaymentId",
                table: "Orders",
                column: "UserPaymentPaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserPayments_UserPaymentPaymentId",
                table: "Orders",
                column: "UserPaymentPaymentId",
                principalTable: "UserPayments",
                principalColumn: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserPayments_UserPaymentPaymentId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserPaymentPaymentId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserPaymentPaymentId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "AverageRating",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RandomReview",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 2,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 3,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 4,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 5,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 6,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 7,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 8,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 9,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 10,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 11,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 12,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 13,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 14,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 15,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 16,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 17,
                columns: new[] { "AverageRating", "RandomReview" },
                values: new object[] { null, null });
        }
    }
}
