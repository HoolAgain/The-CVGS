namespace CVGS_PROG3050.Models
{
    public class OrderViewModel
    {
        public int OrderId {  get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
