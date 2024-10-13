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
                LastName = ""
            };

            var context = new ValidationContext(profileModel, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(profileModel, context, results, true);

            // Assert
            Assert.False(isValid);
        }
    }
}
