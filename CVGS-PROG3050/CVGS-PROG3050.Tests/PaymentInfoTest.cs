using CVGS_PROG3050.Models;
using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Tests
{
    public class PaymentInfoTest
    {
        [Fact]
        public void InvalidPaymentInformation()
        {
            // Arrange
            var paymentModel = new PaymentViewModel
            {
                NameOnCard = "John Doe",
                CardNumber = "1234", // Invalid
                CVVCode = "12", // Invalid
                ExpirationDate = "1/24" // Invalid
            };

            var context = new ValidationContext(paymentModel, null, null);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(paymentModel, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, v => v.ErrorMessage.Contains("Please enter a valid card number, must be 16 digits"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Please enter a valid CVV, must be 3 digits"));
            Assert.Contains(results, v => v.ErrorMessage.Contains("Please enter a valid expiration date, MM/YY"));
        }
    }
}
