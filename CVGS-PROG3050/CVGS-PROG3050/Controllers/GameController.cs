using CVGS_PROG3050.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CVGS_PROG3050.Controllers
{
    public class GameController : Controller
    {

        private readonly VaporDbContext _db;
        public GameController(VaporDbContext context) {
            _db = context;
        }
        public IActionResult AllGamesView()
        {
            var games = _db.Games.ToList();
            return View(games);
        }
    }
}
