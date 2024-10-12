using System.ComponentModel.DataAnnotations;
namespace CVGS_PROG3050.Entities
{
    public class UserPayment
    {
        public int PaymentId { get; set; }
        public string? UserId { get; set; } // Foreign key to User
        public string? NameOnCard { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpirationDate { get; set; }
        public string? CVVCode { get; set; }

        public User? User { get; set; }
    }
}
