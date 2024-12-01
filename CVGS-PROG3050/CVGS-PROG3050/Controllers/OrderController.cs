﻿using CVGS_PROG3050.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CVGS_PROG3050.Entities;
using System.Text;
using CVGS_PROG3050.Models;

namespace CVGS_PROG3050.Controllers
{
    public class OrderController : Controller
    {
        private readonly VaporDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(VaporDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> PurchasedGames()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var purchasedGames = _context.Orders
                .Where(o => o.UserId == user.Id)
                .Select(o => new PurchasedGameViewModel
                {
                    GameName = o.Game.Name,
                    OrderDate = o.OrderDate
                })
                .ToList();

            return View(purchasedGames);
        }

        [HttpPost]
        public IActionResult DownloadGameFile(string gameName)
        {
            if (string.IsNullOrEmpty(gameName))
            {
                return BadRequest("Invalid Game Name");
            }

            var filename = $"{gameName.Replace(" ", "_")}.txt";
            var fileContent = $"Thank you for purchasing {gameName}!\nEnjoy your game!";
            var fileBytes = Encoding.UTF8.GetBytes(fileContent);

            return File(fileBytes, "text/plain", filename);
        }
    }
}