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
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool PromotionalEmails { get; set; }
        public PreferencesViewModel Preferences { get; set; }
        public AddressViewModel Address { get; set; }
        public PaymentViewModel Payment { get; set; }


    }
}
