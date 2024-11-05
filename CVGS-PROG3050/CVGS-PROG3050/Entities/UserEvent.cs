namespace CVGS_PROG3050.Entities
{
    public class UserEvent
    {
        public int Id { get; set; }
        public int EventId { get; set; } // Foreign Key to Event
        public string? UserId { get; set; } // Foreign Key to User
        
        public Event? Event { get; set; }
        public User? User { get; set; }

    }
}
