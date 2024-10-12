/* AccountController.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CVGS_PROG3050.DataAccess;

using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace CVGS_PROG3050.Controllers
{
    public class AccountController : Controller
    {
        private readonly VaporDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AccountController(VaporDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
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
            var user = await _userManager.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null) 
            {
                return RedirectToAction("Login");
            }

            Address mailingAddress = null;
            Address shippingAddress = null;
            UserPayment userPayment = user.UserPayments?.FirstOrDefault();

            if (user.Addresses != null)
            {
                foreach (var adrs in user.Addresses)
                {
                    if (adrs.MailingAddress == true)
                    {
                        mailingAddress = adrs;
                    }
                    if (adrs.ShippingAddress == true)
                    {
                        shippingAddress = adrs;
                    }
                }
            }

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                PromotionalEmails = user.PromotionalEmails,

                FavoritePlatform = user.FavoritePlatform,
                FavoriteCategory = user.FavoriteCategory,
                LanguagePreference = user.LanguagePreference,

                // Mailing Address
                Country = mailingAddress?.Country,
                FullName = mailingAddress?.FullName,
                PhoneNumber = mailingAddress?.PhoneNumber,
                StreetAddress = mailingAddress?.StreetAddress,
                Address2 = mailingAddress?.Address2,
                City = mailingAddress?.City,
                Province = mailingAddress?.Province,
                PostalCode = mailingAddress?.PostalCode,
                DeliveryInstructions = mailingAddress?.DeliveryInstructions,
                
                // Shipping Address
                ShippingCountry = shippingAddress?.Country,
                ShippingFullName = shippingAddress?.FullName,
                ShippingPhoneNumber = shippingAddress?.PhoneNumber,
                ShippingStreetAddress = shippingAddress?.StreetAddress,
                ShippingAddress2 = shippingAddress?.Address2,
                ShippingCity = shippingAddress?.City,
                ShippingProvince = shippingAddress?.Province,
                ShippingPostalCode = shippingAddress?.PostalCode,
                ShippingDeliveryInstructions = shippingAddress?.DeliveryInstructions,

                //Billing info WIP -> Not auto populating user's billing info
                NameOnCard = userPayment?.NameOnCard,
                CardNumber = userPayment?.CardNumber,
                ExpirationDate = userPayment?.ExpirationDate,
                CVVCode = userPayment?.CVVCode,

            };
            return View("profileview", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var item in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine("ModelState Error" + state.Key + item.ErrorMessage);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                user.BirthDate = model.BirthDate;

                user.PromotionalEmails = (bool)model.PromotionalEmails;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }

            }
            return View("Profile", model);
        }

        [HttpPost]
        public async Task<IActionResult> UserPreferences(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                user.FavoriteCategory = model.FavoriteCategory;
                user.FavoritePlatform = model.FavoritePlatform;
                user.LanguagePreference = model.LanguagePreference;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View("Profile", model);
        }

        [HttpPost]
        public async Task<IActionResult> UserShipping(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync (User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            

            if (ModelState.IsValid)
            {
                if (user.Addresses == null)
                {
                    user.Addresses = new List<Address>();
                }

                if (model.MailingSameAsShipping)
                {


                    Address address = null;
                    foreach (var adrs in user.Addresses)
                    {
                        if (adrs.MailingAddress == true || adrs.ShippingAddress == true)
                        {
                            address = adrs;
                            break;
                        }
                    }

                    if (address == null)
                    {
                        address = new Address { UserId = user.Id };
                        user.Addresses.Add(address);
                    }
                    address.UserId = user.Id;
                    address.MailingAddress = true;
                    address.ShippingAddress = true;
                    address.StreetAddress = model.StreetAddress;
                    address.FullName = model.FullName;
                    address.PhoneNumber = model.PhoneNumber;
                    address.Address2 = model.Address2;
                    address.City = model.City;
                    address.Province = model.Province;
                    address.PostalCode = model.PostalCode;
                    address.Country = model.Country;
                    address.DeliveryInstructions = model.DeliveryInstructions;
                }
                else
                {
                    Address mailingAddress = null;
                    foreach (var adrs in user.Addresses)
                    {
                        if (adrs.MailingAddress == true)
                        {
                            mailingAddress = adrs;
                            break;
                        }
                    }

                    if (mailingAddress == null)
                    {
                        mailingAddress = new Address { UserId = user.Id, MailingAddress = true };
                        user.Addresses.Add(mailingAddress);
                    }

                    mailingAddress.UserId = user.Id;
                    mailingAddress.StreetAddress = model.StreetAddress;
                    mailingAddress.FullName = model.FullName;
                    mailingAddress.Address2 = model.Address2;
                    mailingAddress.City = model.City;
                    mailingAddress.Province = model.Province;
                    mailingAddress.PostalCode = model.PostalCode;
                    mailingAddress.Country = model.Country;
                    mailingAddress.DeliveryInstructions = model.DeliveryInstructions;

                    Address shippingAddress = null;
                    foreach (var adrs in user.Addresses)
                    {
                        if (adrs.ShippingAddress == true)
                        {
                            shippingAddress = adrs;
                            break;
                        }
                    }

                    if (shippingAddress == null)
                    {
                        shippingAddress = new Address { UserId = user.Id, ShippingAddress = true };
                        user.Addresses.Add(shippingAddress);
                    }
                    shippingAddress.UserId = user.Id;
                    shippingAddress.Country = model.ShippingCountry;
                    shippingAddress.FullName = model.ShippingFullName;
                    shippingAddress.PhoneNumber = model.ShippingPhoneNumber;
                    shippingAddress.StreetAddress = model.ShippingStreetAddress;
                    shippingAddress.Address2 = model.ShippingAddress2;
                    shippingAddress.City = model.ShippingCity;
                    shippingAddress.Province = model.ShippingProvince;
                    shippingAddress.PostalCode = model.ShippingPostalCode;
                    shippingAddress.DeliveryInstructions = model.ShippingDeliveryInstructions;
                }
                

                var result = await _userManager.UpdateAsync(user);
                foreach (var addr in user.Addresses)
                {
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View("profileview", model);
        }

        [HttpGet]
        public IActionResult CardAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CardAdd(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var creditCard = new UserPayment
                {
                    UserId = user.Id,
                    NameOnCard = model.NameOnCard,
                    CardNumber = model.CardNumber,
                    ExpirationDate = model.ExpirationDate,
                    CVVCode = model.CVVCode
                };
                _context.Add(creditCard);
                await _context.SaveChangesAsync();

                return RedirectToAction("Profile", "Account");
            }

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"ModelState Error: {state.Key} - {error.ErrorMessage}");
                    }
                }
            }

            return View("profileview", model);
        }

    }


}
