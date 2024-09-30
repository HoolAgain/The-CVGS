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
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);

            // this area for seeding data such as games
        }



    }
}