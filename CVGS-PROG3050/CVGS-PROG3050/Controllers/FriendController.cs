using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        }/*

        [HttpPost]
        public async Task<IActionResult> AddFriend(string friendUsername)
        {
            if (string.IsNullOrEmpty(friendUsername))
            {
                TempData["FriendMessage"] = $"User '{friendUsername}' was not found";
                return RedirectToAction("profileView", "Account");
            }

            var userId = _userManager.GetUserId(User);
            var friend = await _userManager.FindByNameAsync(friendUsername);

            if (friend == null)
            {
                TempData["FriendMessage"] = $"User '{friendUsername}' was not found";
                return RedirectToAction("profileView", "Account");
            }
            var friendship = await _db.Friends.AnyAsync(f => (f.UserId == userId && f.FriendUserId == friend.Id) || (f.UserId == friend.Id && f.FriendUserId == userId));
            
        }
*/
        public IActionResult Index()
        {
            return View();
        }
    }
}
