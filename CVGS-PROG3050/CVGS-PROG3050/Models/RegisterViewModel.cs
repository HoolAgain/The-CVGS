/* ReigsterViewModel.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CVGS_PROG3050.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        [StringLength(200)]
        public string? UserName { get; set;}

        [Required(ErrorMessage = "Please enter an email")]
        [EmailAddress] 
        public string? Email { get; set;}

        [Required(ErrorMessage = "Please enter a  password")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string? Password { get; set;}

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set;}
    }
}
