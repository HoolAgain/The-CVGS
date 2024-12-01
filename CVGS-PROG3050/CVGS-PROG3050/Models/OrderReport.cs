namespace CVGS_PROG3050.Models
{
    public class OrderReport
    {
        public DateTime OrderDate { get; set; }
        public string? UserName { get; set; }
        public string? GameName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax {  get; set; }
        public decimal GrandTotal { get; set; }
        public string? Status { get; set; }
        public string? ShipPhysicalCopy { get; set; }
        public decimal ShippingCost { get; set; }
 
    }
}
