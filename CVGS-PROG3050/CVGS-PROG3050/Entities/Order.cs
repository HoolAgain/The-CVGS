namespace CVGS_PROG3050.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string? UserId { get; set; } // Foreign Key to User
        public int GameId { get; set; } // Foreign Key to Game
        public int PaymentId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Status { get; set; }
        public bool ShipPhysicalCopy { get; set; }
        public decimal ShippingCost { get; set; }

        public User? User { get; set; }
        public Game? Game { get; set; }
        public UserPayment? UserPayment { get; set; }

    }
}
