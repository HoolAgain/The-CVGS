using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS_PROG3050.Entities
{
    public class Friend
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FriendUserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("FriendUserId")]
        public User? FriendUser {get; set;}
        
    }
}
