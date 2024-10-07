/* User.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/
using Microsoft.AspNetCore.Identity;

namespace CVGS_PROG3050.Entities
{
    public class User : IdentityUser
    {
        // Profile
        //public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool PromotionalEmails { get; set; }
        // Preferences
        public string? FavoritePlatform { get; set; }
        public string? FavoriteCategory { get; set; }
        public string? LanguagePreference { get; set; }
        public ICollection<Address>? Addresses { get; set; }
    }
}
