using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS_PROG3050.Migrations
{
    /// <inheritdoc />
    public partial class AddReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_FriendUserId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_UserId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_AspNetUsers_UserId",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Games_GameId",
                table: "Wishlist");

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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Friends",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FriendUserId",
                table: "Friends",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_FriendUserId",
                table: "Friends",
                column: "FriendUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_UserId",
                table: "Friends",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_AspNetUsers_UserId",
                table: "Wishlist",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Games_GameId",
                table: "Wishlist",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_FriendUserId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_UserId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_AspNetUsers_UserId",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Games_GameId",
                table: "Wishlist");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RandomReview",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Friends",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FriendUserId",
                table: "Friends",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_FriendUserId",
                table: "Friends",
                column: "FriendUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_UserId",
                table: "Friends",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_AspNetUsers_UserId",
                table: "Wishlist",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Games_GameId",
                table: "Wishlist",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
