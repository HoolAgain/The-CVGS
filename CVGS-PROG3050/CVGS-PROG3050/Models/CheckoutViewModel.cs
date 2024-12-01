namespace CVGS_PROG3050.Models
{
    public class CheckoutViewModel
    {
        public List<CartViewModel> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
