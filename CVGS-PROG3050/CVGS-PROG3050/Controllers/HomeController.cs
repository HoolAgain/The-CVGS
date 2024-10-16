/* HomeController.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Edited
*/
using CVGS_PROG3050.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CVGS_PROG3050.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
