using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System.Collections.Generic;
using System.Reflection.Emit;
using CVGS_PROG3050.Entities;

namespace CVGS_PROG3050.DataAccess
{
    public class VaporDbContext : IdentityDbContext<User>
    {
        public VaporDbContext(DbContextOptions<VaporDbContext> options) : base(options) 
        {
        }

        //This spot will be for databases related to games, wishlists etc
        //public DbSet<Game> Games { get; set; }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserPayment> UserPayments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<Friend> Friends { get; set; }

        // To create admin users
        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string username = "vaporAdmin";
            string password = "vaporTest123!";
            string roleName = "Admin";

            // if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded) 
                {
                    Console.WriteLine("error creating role" + roleResult.Errors.ToString());
                    return;
                }
            }
      
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    Console.WriteLine("Admin user created successfully.");
                    var addingToRole = await userManager.AddToRoleAsync(user, roleName);

                    if (!addingToRole.Succeeded)
                    {
                        Console.WriteLine("Error adding user to role" + addingToRole.Errors.ToString());
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine("ERROR" + error.Description);
                    }
                    Console.WriteLine("FAILED");
                }
            }
            Console.WriteLine("Admin already exists");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>()
                .HasKey(a => a.Id);
            builder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);

            // Configuration for Game
            builder.Entity<Game>()
                .HasKey(g => g.GameId);

            // Configuration for Order
            builder.Entity<Order>()
                .HasKey(o => o.OrderId);
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
            builder.Entity<Order>()
                .HasOne(o => o.Game)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.GameId);

            // Configuration for Wishlist
            builder.Entity<Wishlist>()
                .HasKey(w => new {w.GameId, w.UserId});
            builder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Wishlist>()
                .HasOne(w => w.Game)
                .WithMany(g => g.Wishlists)
                .HasForeignKey(w => w.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration for Rating
            builder.Entity<Rating>()
                .HasKey(r => r.RatingId);
            builder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId);
            builder.Entity<Rating>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Ratings)
                .HasForeignKey(r => r.GameId);

            // Configuration for Review
            builder.Entity<Review>()
                .HasKey(r => r.ReviewId);
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.UserId);
            builder.Entity<Review>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId);

            // Configuration for UserPayment
            builder.Entity<UserPayment>()
                .HasKey(up => up.PaymentId);
            builder.Entity<UserPayment>()
                .HasOne(u => u.User)
                .WithMany(up => up.UserPayments)
                .HasForeignKey(up => up.UserId);

            // Configuarion for Cart
            builder.Entity<Cart>()
                .HasKey(w => new { w.GameId, w.UserId });
            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId);
            builder.Entity<Cart>()
                .HasOne(c => c.Game)
                .WithMany(g => g.Carts)
                .HasForeignKey(c => c.GameId);

            // Configuration for Event
            builder.Entity<Event>()
                .HasKey(e => e.EventId);

            // Configuartion for UserEvent
            builder.Entity<UserEvent>()
                .HasKey(me => new { me.EventId, me.UserId });
            builder.Entity<UserEvent>()
                .HasOne(me => me.Event)
                .WithMany(e => e.UserEvents)
                .HasForeignKey(me => me.EventId);
            builder.Entity<UserEvent>()
                .HasOne(me => me.User)
                .WithMany(u => u.UserEvents)
                .HasForeignKey(me => me.UserId);



            // Seeding the Database with Games
            builder.Entity<Game>().HasData(
                
                new Game()
                {
                    GameId = 1,
                    Name = "Palworld",
                    Genre = "Adventure",
                    ReleaseDate = new DateTime(2024, 1, 19),
                    Developer = "Pocketpair",
                    Publisher = "Pocketpair",
                    Description = "Fight, farm, build and work alongside mysterious creatures called \"Pals\" in this completely new multiplayer, open world survival and crafting game!",
                    Price = 38.99m
                },
                new Game()
                {
                    GameId = 2,
                    Name = "Destiny 2",
                    Genre = "Role Playing",
                    ReleaseDate = new DateTime(2019, 10, 1),
                    Developer = "Bungie",
                    Publisher = "Bungie",
                    Description = "Destiny 2 is an action MMO with a single evolving world that you and your friends can join anytime, anywhere.",
                    Price = 9.99m
                },
                new Game()
                {
                    GameId = 3,
                    Name = "Elden Ring",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2022, 2, 24),
                    Developer = "FromSoftware, Inc.",
                    Publisher = "FromSoftware, Inc., Bandai Namco Entertainment",
                    Description = "THE CRITICALLY ACCLAIMED FANTASY ACTION RPG. Rise, Tarnished, and be guided by grace to brandish the power of the Elden Ring and become an Elden Lord in the Lands Between.",
                    Price = 79.99m
                },
                new Game()
                {
                    GameId = 4,
                    Name = "Fallout 4",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2015, 11, 10),
                    Developer = "Bethesda Game Studios",
                    Publisher = "Bethesda Softworks",
                    Description = "Bethesda Game Studios, the award-winning creators of Fallout 3 and The Elder Scrolls V: Skyrim, welcome you to the world of Fallout 4 – their most ambitious game ever, and the next generation of open-world gaming.",
                    Price = 26.99m
                },
                new Game()
                {
                    GameId = 5,
                    Name = "Forza Horizon 5",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2021, 11, 8),
                    Developer = "Playground Games",
                    Publisher = "Xbox Game Studios",
                    Description = "Explore the vibrant open world landscapes of Mexico with limitless, fun driving action in the world’s greatest cars. Join a thrilling game of chase with our new 5v1 Multiplayer Experience: Hide & Seek.",
                    Price = 79.99m
                },
                new Game()
                {
                    GameId = 6,
                    Name = "Minecraft",
                    Genre = "Adventure",
                    ReleaseDate = new DateTime(2009, 5, 17),
                    Developer = "Mojang Studios",
                    Publisher = "Mojang Studios",
                    Description = "Minecraft is a game made up of blocks, creatures, and community. You can survive the night or build a work of art – the choice is all yours. But if the thought of exploring a vast new world all on your own feels overwhelming, then fear not! Let’s explore what Minecraft is all about!",
                    Price = 29.99m
                },
                new Game()
                {
                    GameId = 7,
                    Name = "LEGO: Harry Potter",
                    Genre = "Adventure",
                    ReleaseDate = new DateTime(2024, 10, 8),
                    Developer = "TT Games, Double Eleven",
                    Publisher = "Warner Bros. Games",
                    Description = "The LEGO® Harry Potter™: Collection brings LEGO® Harry Potter™: Years 1-4 and LEGO® Harry Potter™: Years 5-7 together in one game, now remastered with enhanced graphics.",
                    Price = 49.99m
                },
                new Game()
                {
                    GameId = 8,
                    Name = "LEGO: Star Wars",
                    Genre = "Adventure",
                    ReleaseDate = new DateTime(2022, 4, 5),
                    Developer = "TT Games",
                    Publisher = "Warner Bros. Games, Warner Bros. Interactive Entertainment",
                    Description = "Play through all nine Skywalker saga films in a game unlike any other. With over 300 playable characters, over 100 vehicles, and 23 planets to explore, a galaxy far, far away has never been more fun! *Includes classic Obi-Wan Kenobi playable character",
                    Price = 59.99m
                },
                new Game()
                {
                    GameId = 9,
                    Name = "Monster Hunter: WORLDS",
                    Genre = "Role Playing",
                    ReleaseDate = new DateTime(2018, 8, 8),
                    Developer = "CAPCOM Co., Ltd.",
                    Publisher = "CAPCOM Co., Ltd.",
                    Description = "Welcome to a new world! In Monster Hunter: World, the latest installment in the series, you can enjoy the ultimate hunting experience, using everything at your disposal to hunt monsters in a new world teeming with surprises and excitement.",
                    Price = 39.99m
                },
                new Game()
                {
                    GameId = 10,
                    Name = "Nioh: Complete Edition",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2017, 11, 7),
                    Developer = "KOEI TECMO GAMES CO., LTD.",
                    Publisher = "KOEI TECMO GAMES CO., LTD.",
                    Description = "Ready to die? Experience the newest brutal action game from Team NINJA and Koei Tecmo Games. In the age of samurai, a lone traveler lands on the shores of Japan. He must fight his way through the vicious warriors and supernatural Yokai that infest the land in order to find that which he seeks.",
                    Price = 64.99m
                },
                new Game()
                {
                    GameId = 11,
                    Name = "Overwatch 2",
                    Genre = "Role Playing",
                    ReleaseDate = new DateTime(2023, 8, 10),
                    Developer = "Blizzard Entertainment, Inc.",
                    Publisher = "Blizzard Entertainment, Inc.",
                    Description = "Overwatch 2 is a critically acclaimed, team-based shooter game set in an optimistic future with an evolving roster of heroes. Team up with friends and jump in today.",
                    Price = 9.99m
                },
                new Game()
                {
                    GameId = 12,
                    Name = "Risk of Rain 2",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2020, 8, 11),
                    Developer = "Hopoo Games",
                    Publisher = "Gearbox Publishing",
                    Description = "Escape a chaotic alien planet by fighting through hordes of frenzied monsters – with your friends, or on your own. Combine loot in surprising ways and master each character until you become the havoc you feared upon your first crash landing.",
                    Price = 28.99m
                },
                new Game()
                {
                    GameId = 13,
                    Name = "Street Fighter 6",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2023, 6, 2),
                    Developer = "CAPCOM Co., Ltd.",
                    Publisher = "CAPCOM Co., Ltd.",
                    Description = "Here comes Capcom’s newest challenger! Street Fighter™ 6 launches worldwide on June 2nd, 2023 and represents the next evolution of the Street Fighter™ series! Street Fighter 6 spans three distinct game modes, including World Tour, Fighting Ground and Battle Hub.",
                    Price = 79.99m
                },
                new Game()
                {
                    GameId = 14,
                    Name = "The Elder Scrolls V: Skyrim",
                    Genre = "Role Playing",
                    ReleaseDate = new DateTime(2016, 10, 27),
                    Developer = "Bethesda Game Studios",
                    Publisher = "Bethesda Softworks",
                    Description = "Winner of more than 200 Game of the Year Awards, The Elder Scrolls V: Skyrim Special Edition brings the epic fantasy to life in stunning detail. The Special Edition includes the critically acclaimed game and add-ons with all-new features.",
                    Price = 54.99m
                },
                new Game()
                {
                    GameId = 15,
                    Name = "StarField",
                    Genre = "Role Playing",
                    ReleaseDate = new DateTime(2023, 9, 5),
                    Developer = "Bethesda Game Studios",
                    Publisher = "Bethesda Softworks",
                    Description = "Starfield is the first new universe in 25 years from Bethesda Game Studios, the award-winning creators of The Elder Scrolls V: Skyrim and Fallout 4.",
                    Price = 89.99m
                },
                new Game()
                {
                    GameId = 16,
                    Name = "Tekken 8",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2024, 1, 25),
                    Developer = "Bandai Namco Studios Inc.",
                    Publisher = "Bandai Namco Entertainment",
                    Description = "Get ready for the next chapter in the legendary fighting game franchise, TEKKEN 8.",
                    Price = 93.49m
                },
                new Game()
                {
                    GameId = 17,
                    Name = "The King of Fighters XV",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2022, 2, 17),
                    Developer = "SNK CORPORATION",
                    Publisher = "SNK CORPORATION",
                    Description = "SHATTER ALL EXPECTATIONS! Transcend beyond your limits with KOF XV!",
                    Price = 79.99m
                }
            );


        }



    }
}