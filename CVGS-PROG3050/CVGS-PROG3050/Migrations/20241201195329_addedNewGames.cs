using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS_PROG3050.Migrations
{
    /// <inheritdoc />
    public partial class addedNewGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1,
                columns: new[] { "Description", "Developer", "Name", "Price", "Publisher", "ReleaseDate" },
                values: new object[] { "Fight, farm, build and work alongside mysterious creatures called \"Pals\" in this completely new multiplayer, open world survival and crafting game!", "Pocketpair", "Palworld", 38.99m, "Pocketpair", new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 6,
                columns: new[] { "Description", "Developer", "Name", "Price", "Publisher", "ReleaseDate" },
                values: new object[] { "Minecraft is a game made up of blocks, creatures, and community. You can survive the night or build a work of art – the choice is all yours. But if the thought of exploring a vast new world all on your own feels overwhelming, then fear not! Let’s explore what Minecraft is all about!", "Mojang Studios", "Minecraft", 29.99m, "Mojang Studios", new DateTime(2009, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 15,
                columns: new[] { "Description", "Developer", "Name", "Price", "Publisher", "ReleaseDate" },
                values: new object[] { "Starfield is the first new universe in 25 years from Bethesda Game Studios, the award-winning creators of The Elder Scrolls V: Skyrim and Fallout 4.", "Bethesda Game Studios", "StarField", 89.99m, "Bethesda Softworks", new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 1,
                columns: new[] { "Description", "Developer", "Name", "Price", "Publisher", "ReleaseDate" },
                values: new object[] { "Dead Cells is a roguelite, metroidvania inspired, action-platformer. You'll explore a sprawling, ever-changing castle... assuming you’re able to fight your way past its keepers in 2D souls-lite combat. No checkpoints. Kill, die, learn, repeat.", "Motion Twin", "Dead Cells", 29.99m, "Motion Twin", new DateTime(2018, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 6,
                columns: new[] { "Description", "Developer", "Name", "Price", "Publisher", "ReleaseDate" },
                values: new object[] { "When all the villains in Arkham Asylum team up and break loose, only the dynamic duo is bold enough to take them on to save Gotham City. The fun of LEGO, the drama of Batman and the uniqueness of the combination makes for a comical and exciting adventure in LEGO Batman: The Videogame.", "Traveller's Tale", "LEGO: Batman", 19.99m, "Warner Bros. Interactive Entertainment", new DateTime(2008, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 15,
                columns: new[] { "Description", "Developer", "Name", "Price", "Publisher", "ReleaseDate" },
                values: new object[] { "You've inherited your grandfather's old farm plot in Stardew Valley. Armed with hand-me-down tools and a few coins, you set out to begin your new life. Can you learn to live off the land and turn these overgrown fields into a thriving home?", "ConcernedApe", "Stardew Valley", 16.99m, "ConcernedApe", new DateTime(2016, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
