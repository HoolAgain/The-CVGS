/* Address.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/
namespace CVGS_PROG3050.Entities
{
    public class Address
    {
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? DeliveryInstructions { get; set; }

        public bool? MailingAddress { get; set; }
        public bool? ShippingAddress { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
