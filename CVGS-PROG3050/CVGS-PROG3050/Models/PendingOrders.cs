namespace CVGS_PROG3050.Models
{
    public class PendingOrders
    {
        public int OrderId { get; set; }
        public string? UserName { get; set; }
        public string? GameName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
