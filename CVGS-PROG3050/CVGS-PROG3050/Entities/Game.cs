using System.ComponentModel.DataAnnotations.Schema;

namespace CVGS_PROG3050.Entities
{
    public class Game
    {
        public int GameId { get; set; }
        public string? Name { get; set; }
        public string? Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Developer {  get; set; }
        public string? Publisher { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }



        public ICollection<Order>? Orders { get; set; }
        public bool InWishlist { get; set; }
        public ICollection<Wishlist>? Wishlists { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Cart>? Carts { get; set; }

        [NotMapped]
        public string? AverageRating { get; set; }

        [NotMapped]
        public string? RandomReview { get; set; }

    }
}
