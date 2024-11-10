using CVGS_PROG3050.Models;
using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Tests
{
    public class ProfileTest
    {
        [Fact]
        public void InvalidProfileInformation()
        {
            // Arrange
            var profileModel = new ProfileViewModel
            {
                // Missing fields
                Email = "",
                FirstName = "", 
                LastName = "",
                BirthDate = new DateTime(2025, 10, 01)
            };

            var context = new ValidationContext(profileModel, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(profileModel, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage.Contains("Please enter a valid email address"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("First name must be at least 1 character long"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Last name must be at least 1 character long"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Birth date cannot be in the future"));
        }
    }
}
