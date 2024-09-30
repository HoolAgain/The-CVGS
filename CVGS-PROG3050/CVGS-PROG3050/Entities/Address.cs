﻿/* Address.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/
namespace CVGS_PROG3050.Entities
{
    public class Address
    {
        public int? Id { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        public bool? MailingAddress { get; set; }
        public bool? ShippingAddress { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
