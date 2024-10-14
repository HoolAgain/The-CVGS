using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
