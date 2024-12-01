using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CVGS_PROG3050.DataAccess;
using System.Linq;
using System.Web;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DNTCaptcha.Core;
using CaptchaMvc.Attributes;
using CaptchaMvc.Infrastructure;
using CaptchaMvc.HtmlHelpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace CVGS_PROG3050.Controllers
{
    public class CartController : Controller
    {

        private readonly VaporDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GameController> _logger;

        public CartController(VaporDbContext context, UserManager<User> userManager, ILogger<GameController> logger)
        {
            _db = context;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int gameId)
        {
            var userId = _userManager.GetUserId(User);
            var cartItem = await _db.Carts.FirstOrDefaultAsync(w => w.UserId == userId && w.GameId == gameId);
            bool added = false;
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    UserId = userId,
                    GameId = gameId
                };


                _db.Carts.Add(cartItem);
                added = true;
                TempData["CartNotification"] = "Game has been successfully added to your cart!";

            }
            else
            {
                _db.Carts.Remove(cartItem);
                added = false;

                TempData["CartNotification"] = "Game has been successfully removed from your cart";
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("ViewCart");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int gameId)
        {
            var userId = _userManager.GetUserId(User);
            var cartItem = await _db.Carts.FirstOrDefaultAsync(w => w.UserId == userId && w.GameId == gameId);
            if (cartItem != null)
            {
                _db.Carts.Remove(cartItem);
                await _db.SaveChangesAsync();
                TempData["CartNotification"] = "Game has been successfully removed from your cart";
            }
            return RedirectToAction("ViewCart");
        }

        [Authorize]
        [HttpGet]
        public async Task <IActionResult> ViewCart()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _db.Carts
                .Where(w => w.UserId == userId)
                .Include(w => w.Game)
                .Select(w => new CartViewModel
                {
                    GameId = w.GameId,
                    Name = w.Game.Name,
                    Price = w.Game.Price
                }).ToListAsync();

            if (!cartItems.Any())
            {
                TempData["CartNotification"] = "Your cart is empty";
            }

            var userPayments = await _db.UserPayments
                .Where(p => p.UserId == userId)
                .ToListAsync();

            ViewBag.Payments = userPayments;

            return View("cartView", cartItems);
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(int paymentId, bool shipPhysicalCopy = false)
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _db.Carts
                .Where(w => w.UserId == userId)
                .Include(w => w.Game)
                .ToListAsync();
            decimal shippingCost = 0;
            if (!cartItems.Any())
            {
                TempData["CartNotification"] = "Your cart is empty! Please add a game to cart";
                return RedirectToAction("ViewCart");
            }

            var paymentMethod = await _db.UserPayments.FirstOrDefaultAsync(p => p.PaymentId == paymentId && p.UserId == userId);
            if (paymentMethod == null)
            {
                TempData["CartNotification"] = "Invalid payment method, please select a valid card";
                return RedirectToAction("ViewCart");
            }
            if (shipPhysicalCopy)
            {
                shippingCost = 10;
            }

            decimal subtotal = cartItems.Sum(item => item.Game.Price);
            decimal tax = subtotal * 0.13m;
            decimal totalPrice = subtotal + tax + shippingCost;

            Order order = null;

            foreach (var item in cartItems)
            {
                order = new Order
                {
                    UserId = userId,
                    GameId = item.GameId,
                    PaymentId = paymentId, 
                    OrderDate = DateTime.Now,
                    Subtotal = subtotal,
                    Tax = tax,
                    GrandTotal = totalPrice,
                    Status = "Pending",
                    ShipPhysicalCopy = shipPhysicalCopy,
                    ShippingCost = shippingCost
                };
                _db.Orders.Add(order);
            }
            _db.Carts.RemoveRange(cartItems);

            await _db.SaveChangesAsync();
            TempData["CartNotification"] = "Thank for your purchase!";
            return RedirectToAction("orderConfirmation", order);
        }

        [Authorize]
        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                TempData["Error"] = "Order not found";
                return RedirectToAction("Index", "Home");
            }
            var orderViewModel = new OrderViewModel
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalPrice = order.GrandTotal
            };
            return View(orderViewModel);
        }
        
    }
}
