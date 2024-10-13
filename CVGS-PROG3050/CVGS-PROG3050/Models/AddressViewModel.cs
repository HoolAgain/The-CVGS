namespace CVGS_PROG3050.Models
{
    public class AddressViewModel
    {

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
