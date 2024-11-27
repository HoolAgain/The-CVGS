using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CVGS_PROG3050.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Moq;
using SkiaSharp;

namespace CVGS_PROG3050.Controllers
{
    public class GameController : Controller
    {

        private readonly VaporDbContext _db;
        private readonly UserManager<User> _userManager;

        public GameController(VaporDbContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> AllGamesView()
        {
            var userId = _userManager.GetUserId(User);

            var wishlistIds = await _db.Wishlist
                .Where(w => w.UserId == userId)
                .Select(w => w.GameId)
                .ToListAsync();

            var games = await _db.Games.Select(g => new GameViewModel
            {
                GameId = g.GameId,
                Name = g.Name,
                Genre = g.Genre,
                Description = g.Description,
                ReleaseDate = g.ReleaseDate,
                Developer = g.Developer,
                Publisher = g.Publisher,
                Price = g.Price,
                InWishlist = wishlistIds.Contains(g.GameId),
                AverageRating = (_db.Ratings.Where(r => r.GameId == g.GameId).Average(r => (double?)r.Score) ?? 0).ToString("0.0"),
                RandomReview = _db.Reviews.Where(r => r.GameId == g.GameId).OrderBy(r => Guid.NewGuid()).Select(r => r.ReviewText).FirstOrDefault()


            }).ToListAsync();

            foreach (var game in games)
            {
                var averageRating = (_db.Ratings.Where(r => r.GameId == game.GameId).Average(r => (double?)r.Score) ?? 0).ToString("0.0");
                var randomReview = _db.Reviews.Where(r => r.GameId == game.GameId).OrderBy(r => Guid.NewGuid()).Select(r => r.ReviewText).FirstOrDefault();

                Console.WriteLine($"Game: {game.Name}, Average Rating: {averageRating}, Random Review: {randomReview}");

                game.AverageRating = averageRating;
                game.RandomReview = randomReview;
            }

            foreach (var game in games) { Console.WriteLine($"Game: {game.Name}, Average Rating: {game.AverageRating}, Random Review: {game.RandomReview}"); }

            if (games == null || !games.Any())
            {
                games = new List<GameViewModel>();
            }
            return View(games);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int gameId)
        {
            var userId = _userManager.GetUserId(User);
            var game = await _db.Wishlist.FirstOrDefaultAsync(w => w.UserId == userId && w.GameId == gameId);
            bool added = false;
            if (game == null)
            {
                var wishlistGame = new Wishlist
                {
                    UserId = userId,
                    GameId = gameId
                };


                _db.Wishlist.Add(wishlistGame);
                added = true;
                TempData["WishlistNotification"] = "It has been successfully added to your wishlist!";

            }
            else
            {
                _db.Wishlist.Remove(game);
                added = false;

                TempData["WishlistNotification"] = "It has been successfully removed from your wishlist";
            }


            await _db.SaveChangesAsync();

            var games = await _db.Games.Select(g => new GameViewModel
            {
                GameId = g.GameId,
                Name = g.Name,
                Genre = g.Genre,
                Description = g.Description,
                ReleaseDate = g.ReleaseDate,
                Developer = g.Developer,
                Publisher = g.Publisher,
                Price = g.Price,
                InWishlist = _db.Wishlist.Any(w => w.UserId == userId && w.GameId == g.GameId)
            }).ToListAsync();
            return RedirectToAction("Index", "Home");
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int gameId)
        {
            var userId = _userManager.GetUserId(User);
            var game = await _db.Wishlist.FirstOrDefaultAsync(w => w.UserId == userId && w.GameId == gameId);
            bool added = false;
            if (game != null)
            {
                _db.Wishlist.Remove(game);
                await _db.SaveChangesAsync();
                TempData["WishlistNotification"] = "It has been successfully removed from your wishlist";
            }
            var games = await _db.Games.Select(g => new GameViewModel
            {
                GameId = g.GameId,
                Name = g.Name,
                Genre = g.Genre,
                Description = g.Description,
                ReleaseDate = g.ReleaseDate,
                Developer = g.Developer,
                Publisher = g.Publisher,
                Price = g.Price,
                InWishlist = _db.Wishlist.Any(w => w.UserId == userId && w.GameId == g.GameId)
            }).ToListAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ViewWishlist()
        {
            var userId = _userManager.GetUserId(User);
            var wishListItems = _db.Wishlist
                .Where(w => w.UserId == userId)
                .Include(w => w.Game)
                .Select(w => new GameViewModel
                {
                    GameId = w.GameId,
                    Name = w.Game.Name,
                    Genre = w.Game.Genre,
                    Description = w.Game.Description,
                    ReleaseDate = w.Game.ReleaseDate,
                    Developer = w.Game.Developer,
                    Publisher = w.Game.Publisher,
                    Price = w.Game.Price
                }).ToList();

            return View("ViewWishlist", wishListItems);
        }
     

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(int gameId, string ReviewText)
        {


            if (!_db.Games.Any(g => g.GameId == gameId))
            {
                TempData["Error"] = $"The game with ID {gameId} does not exist."; 
                return RedirectToAction("Index", "Home");
            }
                var userId = _userManager.GetUserId(User);
            var review = new Review
            {
                UserId = userId,
                GameId = gameId,
                ReviewText = ReviewText,
            };
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Review added successfully!";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRating(int gameId, int score)
        {
            if (!_db.Games.Any(g => g.GameId == gameId))
            {
                TempData["Error"] = $"The game with ID {gameId} does not exist."; 
                return RedirectToAction("Index", "Home");
             
            }

                var userId = _userManager.GetUserId(User);
            var rating = new Rating
            {
                UserId = userId,
                GameId = gameId,
                Score = score,
            };
            _db.Ratings.Add(rating);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Rating added successfully!";
            return RedirectToAction("Index", "Home");

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetRandomReviewAndAverageRating(int gameId)
        {
            var reviews = await _db.Reviews.Where(r => r.GameId == gameId).ToListAsync();
            var ratings = await _db.Ratings.Where(r => r.GameId == gameId).ToListAsync();

            if (!reviews.Any() || !ratings.Any())
            {
                return NotFound("No reviews or ratings found for this game.");
            }

            var randomReview = reviews[new Random().Next(reviews.Count)];
            var averageRating = ratings.Average(r => r.Score);

            return Ok(new
            {
                RandomReview = randomReview.ReviewText,
                AverageRating = averageRating
            });
        }


        public List<Review> GetAllReviews()
        {
            return _db.Reviews.Include(r => r.Game).ToList(); // Assuming you have a navigation property for Game
        }













    }
}
