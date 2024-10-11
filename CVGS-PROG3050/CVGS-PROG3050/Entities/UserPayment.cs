namespace CVGS_PROG3050.Entities
{
    public class UserPayment
    {
        public int PaymentId { get; set; }
        public string? UserId { get; set; } // Foreign key to User
        public int CardNumber { get; set; }
        public int ExpirationDate { get; set; }
        public int CVVCode { get; set; }

        public User? User { get; set; }
    }
}
