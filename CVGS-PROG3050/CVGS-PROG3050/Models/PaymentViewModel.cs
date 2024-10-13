using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Models
{
    public class PaymentViewModel
    {
        //[Required(ErrorMessage = "Please enter the name on the card")]
        public string? NameOnCard { get; set; }
        //[Required(ErrorMessage = "Please enter the card number")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Please enter a valid card number, must be 16 digits")]
        public string? CardNumber { get; set; }
        //[Required (ErrorMessage = "Please enter the CVV")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Please enter a valid CVV, must be 3 digits")]
        public string? CVVCode { get; set; }
        //[Required(ErrorMessage = "Please enter the expiration date")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Please enter a valid expiration date, MM/YY")]
        public string? ExpirationDate { get; set; }
    }
}
