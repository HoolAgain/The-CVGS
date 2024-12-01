using System.ComponentModel.DataAnnotations;

namespace CVGS_PROG3050.Models
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public string? Name { get; set; }
        public string? Genre { get; set; }

        public string? Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? Developer {  get; set; }

        public string? Publisher { get; set; }

        public decimal Price { get; set; }
        public bool InWishlist { get; set; } = false;

        public string? AverageRating { get; set; }
        public string? RandomReview { get; set; }
        public List<ReviewViewModel>? Reviews { get; set; }

    }
}
