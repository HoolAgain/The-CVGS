/* AccountController.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;

namespace CVGS_PROG3050.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("signupview");
        }

        // Logic for user registering 
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Sorry! This display name has already been taken. Pick another one please.");
                    return View("signupview", model);
                }

                var user = new User { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View("signupview", model);
        }
        // logic for signing out
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        // Logic for logging in
        [HttpGet]
        public IActionResult LogIn(String? returnUrl = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View("loginview",model);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
                User user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Your account has been locked due to 3 failed login attempts. Please reset your password.");
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");

            }
            return View("loginview",model);
        }

        // Logic for profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) 
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel 
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    BirthDate = user.BirthDate,
                    PromotionalEmails = user.PromotionalEmails,
                    FavoritePlatform = user.FavoritePlatform,
                    FavoriteCategory = user.FavoriteCategory
                    
                };
            return View("profileview", model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                user.BirthDate = model.BirthDate;
                user.PromotionalEmails = (bool)model.PromotionalEmails;
                user.FavoritePlatform = model.FavoritePlatform;
                user.FavoriteCategory = model.FavoriteCategory;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }

            }
            return View(model);
        }

    }

}
