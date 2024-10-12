namespace CVGS_PROG3050.Entities
{
    public class Cart
    {
        public string? UserId { get; set; } // Foreign Key to User
        public int GameId { get; set; } // Foreign Key to Game

        public User? User { get; set; }
        public Game? Game { get; set; }

    }
}
