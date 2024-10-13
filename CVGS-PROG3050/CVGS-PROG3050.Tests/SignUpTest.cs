using CVGS_PROG3050.Models;
using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Tests
{
    public class SignUpTest
    {
        [Fact]
        public void TestForMismatchedPasswords()
        {
            // Arrange:
            var model = new RegisterViewModel
            {
                UserName = "newuser",
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "DifferentPassword123!" // Passwords do not match
            };

            var context = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();

            // Act:
            bool isValid = Validator.TryValidateObject(model, context, validationResults, true);

            // Assert:
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("'Password' and 'Confirm Password' do not match."));
        }
    }
}