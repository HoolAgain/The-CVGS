/* ProfileViewModel.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/

using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Models
{
    public class ProfileViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool PromotionalEmails { get; set; }

        // Preferences
        public string? FavoritePlatform { get; set; }
        public string? FavoriteCategory { get; set; }
        public string? LanguagePreference { get; set; }

        // Address info
        public string? Country { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? DeliveryInstructions { get; set; }

        public bool MailingSameAsShipping {  get; set; }


        public string? ShippingCountry { get; set; }
        public string? ShippingFullName { get; set; }
        public string? ShippingPhoneNumber { get; set; }
        public string? ShippingStreetAddress { get; set; }
        public string? ShippingAddress2 { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingProvince { get; set; }
        public string? ShippingPostalCode { get; set; }
        public string? ShippingDeliveryInstructions { get; set; }

        [Required(ErrorMessage = "Please enter the name on the card")]
        public string? NameOnCard { get; set; }
        [Required(ErrorMessage = "Please enter the card number")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Please enter a valid card number, must be 16 digits")]
        public string? CardNumber { get; set; }
        [Required (ErrorMessage = "Please enter the CVV")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Please a valid CVV, must be 3 digits")]
        public string? CVVCode { get; set; }
        [Required(ErrorMessage = "Please enter the expiration date")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Please enter a valid expiration date, MM/YY")]
        public string? ExpirationDate { get; set; }
    }
}
