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
                .HasForeignKey(w => w.UserId);
            builder.Entity<Wishlist>()
                .HasOne(w => w.Game)
                .WithMany(g => g.Wishlists)
                .HasForeignKey(w => w.GameId);

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

            // this area for seeding data such as games
        }



    }
}