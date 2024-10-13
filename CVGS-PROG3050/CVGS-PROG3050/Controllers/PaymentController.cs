//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using CVGS_PROG3050.DataAccess;
//using CVGS_PROG3050.Entities;
//using CVGS_PROG3050.Models;
//using System.Net;
//using Microsoft.EntityFrameworkCore;

//namespace CVGS_PROG3050.Controllers
//{
//    public class PaymentController : Controller
//    {
//        private readonly VaporDbContext _context;
//        private readonly UserManager<User> _userManager;

//        public PaymentController(VaporDbContext context, UserManager<User> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        [HttpGet]
//        public IActionResult Add()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Add(ProfileViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _userManager.GetUserAsync(User);
//                var creditCard = new UserPayment
//                {
//                    UserId = user.Id,
//                    NameOnCard = model.NameOnCard,
//                    CardNumber = model.CardNumber,
//                    ExpirationDate = model.ExpirationDate,
//                    CVVCode = model.CVVCode
//                };
//                _context.Add(creditCard);
//                await _context.SaveChangesAsync();

//                return RedirectToAction("Profile", "Account");
//            }

//            if (!ModelState.IsValid)
//            {
//                foreach (var state in ModelState)
//                {
//                    foreach (var error in state.Value.Errors)
//                    {
//                        System.Diagnostics.Debug.WriteLine($"ModelState Error: {state.Key} - {error.ErrorMessage}");
//                    }
//                }
//            }

//            return View("Profile", "Account");
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
