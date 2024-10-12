using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CVGS_PROG3050.Migrations
{
    /// <inheritdoc />
    public partial class CompletedSeedingDatabaseWithGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "Description", "Developer", "Genre", "Name", "Price", "Publisher", "ReleaseDate" },
                values: new object[,]
                {
                    { 2, "Destiny 2 is an action MMO with a single evolving world that you and your friends can join anytime, anywhere.", "Bungie", "Role Playing", "Destiny 2", 9.99m, "Bungie", new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "THE CRITICALLY ACCLAIMED FANTASY ACTION RPG. Rise, Tarnished, and be guided by grace to brandish the power of the Elden Ring and become an Elden Lord in the Lands Between.", "FromSoftware, Inc.", "Action", "Elden Ring", 79.99m, "FromSoftware, Inc., Bandai Namco Entertainment", new DateTime(2022, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Bethesda Game Studios, the award-winning creators of Fallout 3 and The Elder Scrolls V: Skyrim, welcome you to the world of Fallout 4 – their most ambitious game ever, and the next generation of open-world gaming.", "Bethesda Game Studios", "Action", "Fallout 4", 26.99m, "Bethesda Softworks", new DateTime(2015, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Explore the vibrant open world landscapes of Mexico with limitless, fun driving action in the world’s greatest cars. Join a thrilling game of chase with our new 5v1 Multiplayer Experience: Hide & Seek.", "Playground Games", "Action", "Forza Horizon 5", 79.99m, "Xbox Game Studios", new DateTime(2021, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "When all the villains in Arkham Asylum team up and break loose, only the dynamic duo is bold enough to take them on to save Gotham City. The fun of LEGO, the drama of Batman and the uniqueness of the combination makes for a comical and exciting adventure in LEGO Batman: The Videogame.", "Traveller's Tale", "Adventure", "LEGO: Batman", 19.99m, "Warner Bros. Interactive Entertainment", new DateTime(2008, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "The LEGO® Harry Potter™: Collection brings LEGO® Harry Potter™: Years 1-4 and LEGO® Harry Potter™: Years 5-7 together in one game, now remastered with enhanced graphics.", "TT Games, Double Eleven", "Adventure", "LEGO: Harry Potter", 49.99m, "Warner Bros. Games", new DateTime(2024, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Play through all nine Skywalker saga films in a game unlike any other. With over 300 playable characters, over 100 vehicles, and 23 planets to explore, a galaxy far, far away has never been more fun! *Includes classic Obi-Wan Kenobi playable character", "TT Games", "Adventure", "LEGO: Star Wars", 59.99m, "Warner Bros. Games, Warner Bros. Interactive Entertainment", new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Welcome to a new world! In Monster Hunter: World, the latest installment in the series, you can enjoy the ultimate hunting experience, using everything at your disposal to hunt monsters in a new world teeming with surprises and excitement.", "CAPCOM Co., Ltd.", "Role Playing", "Monster Hunter: WORLDS", 39.99m, "CAPCOM Co., Ltd.", new DateTime(2018, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Ready to die? Experience the newest brutal action game from Team NINJA and Koei Tecmo Games. In the age of samurai, a lone traveler lands on the shores of Japan. He must fight his way through the vicious warriors and supernatural Yokai that infest the land in order to find that which he seeks.", "KOEI TECMO GAMES CO., LTD.", "Action", "Nioh: Complete Edition", 64.99m, "KOEI TECMO GAMES CO., LTD.", new DateTime(2017, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "Overwatch 2 is a critically acclaimed, team-based shooter game set in an optimistic future with an evolving roster of heroes. Team up with friends and jump in today.", "Blizzard Entertainment, Inc.", "Role Playing", "Overwatch 2", 9.99m, "Blizzard Entertainment, Inc.", new DateTime(2023, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "Escape a chaotic alien planet by fighting through hordes of frenzied monsters – with your friends, or on your own. Combine loot in surprising ways and master each character until you become the havoc you feared upon your first crash landing.", "Hopoo Games", "Action", "Risk of Rain 2", 28.99m, "Gearbox Publishing", new DateTime(2020, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "Here comes Capcom’s newest challenger! Street Fighter™ 6 launches worldwide on June 2nd, 2023 and represents the next evolution of the Street Fighter™ series! Street Fighter 6 spans three distinct game modes, including World Tour, Fighting Ground and Battle Hub.", "CAPCOM Co., Ltd.", "Action", "Street Fighter 6", 79.99m, "CAPCOM Co., Ltd.", new DateTime(2023, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "Winner of more than 200 Game of the Year Awards, The Elder Scrolls V: Skyrim Special Edition brings the epic fantasy to life in stunning detail. The Special Edition includes the critically acclaimed game and add-ons with all-new features.", "Bethesda Game Studios", "Role Playing", "The Elder Scrolls V: Skyrim", 54.99m, "Bethesda Softworks", new DateTime(2016, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "You've inherited your grandfather's old farm plot in Stardew Valley. Armed with hand-me-down tools and a few coins, you set out to begin your new life. Can you learn to live off the land and turn these overgrown fields into a thriving home?", "ConcernedApe", "Role Playing", "Stardew Valley", 16.99m, "ConcernedApe", new DateTime(2016, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, "Get ready for the next chapter in the legendary fighting game franchise, TEKKEN 8.", "Bandai Namco Studios Inc.", "Action", "Tekken 8", 93.49m, "Bandai Namco Entertainment", new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, "SHATTER ALL EXPECTATIONS! Transcend beyond your limits with KOF XV!", "SNK CORPORATION", "Action", "The King of Fighters XV", 79.99m, "SNK CORPORATION", new DateTime(2022, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "GameId",
                keyValue: 17);
        }
    }
}
