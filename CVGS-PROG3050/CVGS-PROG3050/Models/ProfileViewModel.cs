/* ProfileViewModel.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/

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

        // Preferences
        public string? FavoritePlatform { get; set; }
        public string? FavoriteCategory { get; set; }
        public string? LanguagePreference { get; set; }

        // Address info
        public string? Country { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? AdditionalComments { get; set; }
        public bool MailingSameAsShipping {  get; set; }
    }
}
