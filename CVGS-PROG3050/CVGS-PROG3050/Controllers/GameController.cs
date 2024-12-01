using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CVGS_PROG3050.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Moq;
using SkiaSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CVGS_PROG3050.Controllers
{
    public class GameController : Controller
    {

        private readonly VaporDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GameController> _logger;

        public GameController(VaporDbContext context, UserManager<User> userManager, ILogger<GameController> logger)
        {
            _db = context;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> AllGamesView()
        {
            var userId = _userManager.GetUserId(User);

            var wishlistIds = await _db.Wishlist
                .Where(w => w.UserId == userId)
                .Select(w => w.GameId)
                .ToListAsync();

            var games = await _db.Games
                .Include(g => g.Ratings)
                .Include(g =>g.Reviews)
                .Select(g => new GameViewModel
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
                AverageRating = (_db.Ratings
                .Where(r => r.GameId == g.GameId)
                .Average(r => (double?)r.Score) ?? 0)
                .ToString("0.0"),
                RandomReview = _db.Reviews
                .Where(r => r.GameId == g.GameId)
                .OrderBy(r => Guid.NewGuid())
                .Select(r => r.ReviewText)
                .FirstOrDefault()


            }).ToListAsync();

           
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
        [HttpPost]
        public async Task<IActionResult> AddGame(Game game)
        {
            if (ModelState.IsValid)
            {
                _db.Games.Add(game);
                await _db.SaveChangesAsync();
                TempData["GameStatus"] = "Game added successfully!";
                return RedirectToAction("AllGamesView");
            }

            TempData["GameStatus"] = "Error adding game. Please check the input.";
            return View(game);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddGameAdmin(AdminPanelViewModel model)
        {
            if (ModelState.IsValid)
            {
                _db.Games.Add(model.Game);
                await _db.SaveChangesAsync();
                TempData["GameStatus"] = "Game added!";
            }
            else
            {
                TempData["GameStatus"] = "Error adding game.";
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditGame(int gameId)
        {
            var game = await _db.Games.FindAsync(gameId);

            if (game == null)

            {
                TempData["Error"] = "Game not found.";

                return RedirectToAction("Index", "Home");
            }
            var model = new AdminPanelViewModel
            {
                Game = game
            };

            return View("EditGame", model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditGameAdmin(int id, AdminPanelViewModel model)
        {

            Console.WriteLine($"Received id: {id}");
            if (ModelState.IsValid)
            {
                var game = await _db.Games.FindAsync(id);

                if (game == null)
                {
                    TempData["GameStatus"] = "Game not found.";
                    return View("EditGame", model);
                }

                game.Name = model.Game.Name;
                game.Genre = model.Game.Genre;
                game.ReleaseDate = model.Game.ReleaseDate;
                game.Developer = model.Game.Developer; 
                game.Publisher = model.Game.Publisher;
                game.Description = model.Game.Description; 
                game.Price = model.Game.Price; 

                await _db.SaveChangesAsync();
                TempData["GameStatus"] = "Game updated!";
                return View("EditGame", model);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["GameStatus"] = "Error updating game." + string.Join(", ", errors);
                return View("EditGame", model);
            }

            
        }

        public List<Review> GetAllReviews()
        {
            return _db.Reviews.Include(r => r.Game).ToList();
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AdminDeleteGame(int gameId)
        {
            var game = await _db.Games.FindAsync(gameId);
            if (game == null)
            {
                TempData["Error"] = $"The game with the id of {gameId} does not exist.";
                return RedirectToAction("Index", "Home");
            }
            _db.Games.Remove(game);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Game deleted!";
            return RedirectToAction("Index", "Home");
        }









    }
}
