namespace CVGS_PROG3050.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string? UserId { get; set; }

        public Game? Game { get; set; }
        public User? User { get; set; }

    }
}
