/* ProfileViewModel.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/

using System.ComponentModel.DataAnnotations;
namespace CVGS_PROG3050.Models
{
    public class ProfileViewModel
    {
        public string? UserName { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }
        [MinLength(1, ErrorMessage = "First name must be at least 1 character long")]
        [StringLength(50, ErrorMessage =("First name must be between 1 and 50 characters"))]
        public string? FirstName { get; set; }
        [MinLength(1, ErrorMessage = "Last name must be at least 1 character long")]
        [StringLength(50, ErrorMessage = ("Last name must be between 1 and 50 characters"))]
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        [Range(typeof(DateTime), "1/1/1930", "10/17/2024", ErrorMessage = "Birth date cannot be in the future")]
        public DateTime? BirthDate { get; set; }
        public bool PromotionalEmails { get; set; }
        public PreferencesViewModel Preferences { get; set; }
        public AddressViewModel Address { get; set; }
        public PaymentViewModel Payment { get; set; }
        public List<FriendViewModel> Friends { get; set; } = new List<FriendViewModel>();
        public List<FriendViewModel> AllUsers { get; set; } = new List<FriendViewModel>();

    }
}
