/* LoginViewModel.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/
using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your display name")]
        [StringLength(200)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
