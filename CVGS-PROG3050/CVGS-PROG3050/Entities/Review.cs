namespace CVGS_PROG3050.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string? UserId { get; set; } // Foreign Key to User
        public int GameId { get; set; } // Foreign Key to Game
        public string? ReviewText { get; set; }

        public User User { get; set; }
        public Game Game { get; set; }

    }
}
