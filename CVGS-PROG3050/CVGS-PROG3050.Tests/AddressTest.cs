using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Tests
{
    public class AddressTest
    {
        [Fact]
        public async Task AddAddressToDatabase()
        {
            // Arrange : Using an in-memory database for testing purposes
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new VaporDbContext(options))
            {
                var controller = new AccountController(context, null, null, null);

                // Creating new Address to be added
                var address = new Address()
                {
                    FullName = "John Doe",
                    PhoneNumber = "1234567890",
                    StreetAddress = "123 Main St",
                    City = "Guelph",
                    Province = "Ontario",
                    PostalCode = "A1B2C3",
                    Country = "Canada",
                    MailingAddress = true,
                    ShippingAddress = false,
                    UserId = "test-user-id"
                };

                // Act: Add the address to the database and save changes
                context.Addresses.Add(address);
                await context.SaveChangesAsync();

                var addedAddress = await context.Addresses.FirstOrDefaultAsync(a => a.UserId == "test-user-id");

                // Assert: Check if the address was added to the database
                Assert.NotNull(addedAddress);
                // Testing for a couple of the Address fields
                Assert.Equal("John Doe", addedAddress.FullName);
                Assert.Equal("123 Main St", addedAddress.StreetAddress);
            }
        }

        [Fact]
        public void MissingAddressFields()
        {
            // Arrange
            var addressModel = new AddressViewModel
            {
                Country = "",
                FullName = "",
                PhoneNumber = "",
                StreetAddress = "",
                City = "",
                Province = "",
                PostalCode = ""
            };

            var context = new ValidationContext(addressModel, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(addressModel, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage.Contains("Please select a country"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Please enter your full name"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Phone number is required, please enter it without any symbols just digits"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Street address is required"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("City is required"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Province is required"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Postal code is required"));
        }
    }
}
