/* HomeController.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Edited
*/
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CVGS_PROG3050.Entities;
using Microsoft.EntityFrameworkCore;

namespace CVGS_PROG3050.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;


        public HomeController(ILogger<HomeController> logger, VaporDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _db = context;
            _userManager = userManager;
        }

        private readonly VaporDbContext _db;
        
        public async Task<IActionResult> Index()
        {
            //var games = _db.Games.ToList();
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
                AverageRating = g.Ratings.Any()
                ? g.Ratings.Average(r => r.Score).ToString("0.0") : "No Ratings Yet",
                RandomReview = _db.Reviews
                .Where(r => r.GameId == g.GameId)
                .OrderBy(r => Guid.NewGuid())
                .Select(r => r.ReviewText)
                .FirstOrDefault()
            }).ToListAsync();
            return View(games);
        }

        [HttpGet]
        public async Task<IActionResult> GetGameReviews(int gameId)
        {
            var reviews = await _db.Reviews
                .Where(r => r.GameId == gameId)
                .Select(r => new
                {
                    reviewText = r.ReviewText,
                    username = r.User.UserName
                }).ToListAsync();

            return Json(reviews);
        }
        public IActionResult EventsView()
        {
            return View();
        }
        public IActionResult AdminPanelView()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult loginview()
        {
            return View();
        }
        public IActionResult signupview()
        {
            return View();
        }
        public IActionResult profileview()
        {
            return View();
        }
        public IActionResult GameInfoview()
        {
            return View();
        }
        public IActionResult SearchResultview()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
