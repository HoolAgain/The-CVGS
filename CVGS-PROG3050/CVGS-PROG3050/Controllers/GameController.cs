using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CVGS_PROG3050.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
                InWishlist = wishlistIds.Contains(g.GameId)
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
            //bool inWishlist = _db.Wishlist
            //    .Where(w => w.UserId == userId && w.GameId == g.GameId)
            //    .Any();
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
    }
}
