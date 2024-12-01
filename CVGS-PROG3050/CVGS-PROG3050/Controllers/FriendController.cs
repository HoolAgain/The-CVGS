using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CVGS_PROG3050.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CVGS_PROG3050.Controllers
{
    public class FriendController : Controller
    {
        private readonly VaporDbContext _db;
        private readonly UserManager<User> _userManager;

        public FriendController(VaporDbContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ViewFriends()
        {
            var userId = _userManager.GetUserId(User);
            var allUsers = await _userManager.Users
                .Where(u => u.Id != userId) // Exclude current user
                .Select(u => new FriendViewModel
                {
                    UserId = u.Id,
                    Username = u.UserName
                })
                .ToListAsync();
            var friends = await _db.Friends
                .Where(f => f.UserId == userId || f.FriendUserId == userId)
                .Select (f => f.UserId == userId ? f.FriendUser : f.User)
                .Select(u => new FriendViewModel
                {
                    UserId = u.Id,
                    Username = u.UserName
                })
                .ToListAsync();
            var model = new ProfileViewModel
            {
                Friends = friends,
                AllUsers = allUsers
            };
            return View("Profile", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(string friendUsername)
        {
            if (string.IsNullOrWhiteSpace(friendUsername))
            {
                TempData["FriendStatus"] = $"Please enter a username";
                return RedirectToAction("Profile", "Account");
            }
            var userId = _userManager.GetUserId(User);
            var friend = await _userManager.FindByNameAsync(friendUsername);

            if (friend == null)
            {
                TempData["FriendStatus"] = $"The user '{friendUsername}' was not found.";
                return RedirectToAction("Profile", "Account");
            }

            var alreadyFriends = await _db.Friends.AnyAsync(f => (f.UserId == userId && f.FriendUserId == friend.Id) || (f.UserId == friend.Id && f.FriendUserId == userId));
            if (alreadyFriends)
            {
                TempData["FriendStatus"] = $"The user '{friendUsername}' is already your friend.";
            }
            else
            {
                var friends = new Friend
                {
                    UserId = userId,
                    FriendUserId = friend.Id
                };

                _db.Friends.Add(friends);
                await _db.SaveChangesAsync();
                TempData["FriendStatus"] = $"The user '{friendUsername}' is now your friend!";
            }

            return RedirectToAction("Profile", "Account");

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewAvailableUsers()
        {
            var userId = _userManager.GetUserId(User);

            var friendsIds = await _db.Friends
                .Where(f => f.UserId == userId || f.FriendUserId == userId)
                .Select(f => f.UserId == userId ? f.FriendUserId : f.UserId)
                .ToListAsync();

            var availableUsers = await _db.Users
                .Where(u => u.Id != userId && !friendsIds.Contains(u.Id))
                .Select(u => new FriendViewModel
                {
                    UserId = u.Id,
                    Username = u.UserName
                })
                .ToListAsync();

            return View(availableUsers);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFriend(string friendId)
        {
            var userId = _userManager.GetUserId(User);
            var friends = await _db.Friends.FirstOrDefaultAsync (f => (f.UserId == userId && f.FriendUserId == friendId) || (f.UserId == friendId && f.FriendUserId == userId));


            if (friends == null)
            {
                TempData["FriendStatus"] = $"This friend doesn't exist";
                return RedirectToAction("Profile", "Account");
            }
            else
            {
                _db.Friends.Remove(friends);
                await _db.SaveChangesAsync();
                TempData["FriendStatus"] = "Friend has been removed";

            }
            return RedirectToAction("Profile", "Account");

        }
    }
}
