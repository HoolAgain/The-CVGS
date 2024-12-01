using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Models
{
    public class AddressViewModel
    {

        // Address info
        [Required(ErrorMessage = "Please select a country")]
        public string? Country { get; set; }
        [Required(ErrorMessage = "Please enter your full name")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Phone number is required, please enter it without any symbols just digits")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10 digit phone number, no symbols needed only digits")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Street address is required")]
        public string? StreetAddress { get; set; }
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Province is required")]
        public string? Province { get; set; }
        [Required(ErrorMessage = "Postal code is required")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Please enter a valid Canadian postal code")]
        public string? PostalCode { get; set; }
        public string? DeliveryInstructions { get; set; }

        public bool MailingSameAsShipping { get; set; }

        public string? ShippingCountry { get; set; }
        public string? ShippingFullName { get; set; }
        public string? ShippingPhoneNumber { get; set; }
        public string? ShippingStreetAddress { get; set; }
        public string? ShippingAddress2 { get; set; }
        public string? ShippingCity { get; set; }
        public string? ShippingProvince { get; set; }
        public string? ShippingPostalCode { get; set; }
        public string? ShippingDeliveryInstructions { get; set; }
    }
}
