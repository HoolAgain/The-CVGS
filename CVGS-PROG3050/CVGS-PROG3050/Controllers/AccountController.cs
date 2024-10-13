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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public IActionResult LogIn(string? returnUrl = "")
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
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.Users
                .Include(u => u.Addresses)
                .Include(u => u.UserPayments)
                .FirstOrDefaultAsync(u => u.Id == userId);

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

            bool mailingSameAsShipping = mailingAddress != null && shippingAddress != null &&
                mailingAddress.Country == shippingAddress.Country &&
                mailingAddress.FullName == shippingAddress.FullName &&
                mailingAddress.PhoneNumber == shippingAddress.PhoneNumber &&
                mailingAddress.StreetAddress == shippingAddress.StreetAddress &&
                mailingAddress.Address2 == shippingAddress.Address2 &&
                mailingAddress.City == shippingAddress.City &&
                mailingAddress.Province == shippingAddress.Province &&
                mailingAddress.PostalCode == shippingAddress.PostalCode &&
                mailingAddress.DeliveryInstructions == shippingAddress.DeliveryInstructions;

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                PromotionalEmails = user.PromotionalEmails,

                Preferences = new PreferencesViewModel 
                {
                    FavoriteCategory = user.FavoriteCategory,
                    FavoritePlatform = user.FavoritePlatform,
                    LanguagePreference = user.LanguagePreference
                },

                // Mailing Address
                Address = new AddressViewModel
                {
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
                    MailingSameAsShipping = mailingSameAsShipping,
                },
                Payment = new PaymentViewModel 
                {
                    NameOnCard = userPayment?.NameOnCard,
                    CardNumber = userPayment?.CardNumber,
                    ExpirationDate = userPayment?.ExpirationDate,
                    CVVCode = userPayment?.CVVCode,
                }
            };
            System.Diagnostics.Debug.WriteLine($"Profile Loaded: Payment Info - NameOnCard = {model.Payment.NameOnCard}, CardNumber = {model.Payment.CardNumber}, ExpirationDate = {model.Payment.ExpirationDate}, CVVCode = {model.Payment.CVVCode}");

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

            ModelState.Remove("Preferences");
            ModelState.Remove("Address");
            ModelState.Remove("Payment");

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
                user.BirthDate = model.BirthDate;
                user.PromotionalEmails = (bool)model.PromotionalEmails;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }

            }
            return View("profileview", model);
        }

        [HttpPost]
        public async Task<IActionResult> UserPreferences(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.Remove("Address");
            ModelState.Remove("Payment");
            

            if (ModelState.IsValid)
            {
                user.FavoriteCategory = model.Preferences.FavoriteCategory;
                user.FavoritePlatform = model.Preferences.FavoritePlatform;
                user.LanguagePreference = model.Preferences.LanguagePreference;

                //_context.Entry(user).State = EntityState.Modified;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    System.Diagnostics.Debug.WriteLine($"Preferences updated successfully: FavoriteCategory = {user.FavoriteCategory}, FavoritePlatform = {user.FavoritePlatform}, LanguagePreference = {user.LanguagePreference}");

                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error updating user: {item.Description}");
                    }
                }
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach(var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"ModelState Error: {state.Key} - {error.ErrorMessage}");

                    }
                }
            }


            return View("profileview", model);
        }

        [HttpPost]
        public async Task<IActionResult> UserShipping(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync (User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.Remove("Preferences");
            ModelState.Remove("Payment");

            if (ModelState.IsValid)
            {
                if (user.Addresses == null)
                {
                    user.Addresses = new List<Address>();
                }

                if (model.Address.MailingSameAsShipping)
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
                        address = new Address { UserId = user.Id, MailingAddress = true, ShippingAddress = true};
                        user.Addresses.Add(address);
                        _context.Add(address);
                    }
                    else
                    {
                        address.MailingAddress = true;
                        address.ShippingAddress = true;
                        _context.Entry(address).State = EntityState.Modified;
                    }
                    address.StreetAddress = model.Address.StreetAddress;
                    address.FullName = model.Address.FullName;
                    address.PhoneNumber = model.Address.PhoneNumber;
                    address.Address2 = model.Address.Address2;
                    address.City = model.Address.City;
                    address.Province = model.Address.Province;
                    address.PostalCode = model.Address.PostalCode;
                    address.Country = model.Address.Country;
                    address.DeliveryInstructions = model.Address.DeliveryInstructions;
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
                        _context.Add(mailingAddress);
                    }
                    else
                    {
                        _context.Entry(mailingAddress).State = EntityState.Modified;
                    }

                    mailingAddress.StreetAddress = model.Address.StreetAddress;
                    mailingAddress.FullName = model.Address.FullName;
                    mailingAddress.PhoneNumber = model.Address.PhoneNumber;
                    mailingAddress.Address2 = model.Address.Address2;
                    mailingAddress.City = model.Address.City;
                    mailingAddress.Province = model.Address.Province;
                    mailingAddress.PostalCode = model.Address.PostalCode;
                    mailingAddress.Country = model.Address.Country;
                    mailingAddress.DeliveryInstructions = model.Address.DeliveryInstructions;



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
                        _context.Add(shippingAddress);
                    }

                    shippingAddress.StreetAddress = model.Address.ShippingStreetAddress;
                    shippingAddress.FullName = model.Address.ShippingFullName;
                    shippingAddress.PhoneNumber = model.Address.ShippingPhoneNumber;
                    shippingAddress.Address2 = model.Address.ShippingAddress2;
                    shippingAddress.City = model.Address.ShippingCity;
                    shippingAddress.Province = model.Address.ShippingProvince;
                    shippingAddress.PostalCode = model.Address.ShippingPostalCode;
                    shippingAddress.Country = model.Address.ShippingCountry;
                    shippingAddress.DeliveryInstructions = model.Address.ShippingDeliveryInstructions;
                }

                await _context.SaveChangesAsync();

                var result = await _userManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Account");
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.Remove("Preferences");
            ModelState.Remove("Address");

            if (ModelState.IsValid)
            {
                if (user.UserPayments == null)
                {
                    user.UserPayments = new List<UserPayment>();
                }

                UserPayment paymentInfo = null;

                foreach (var payment in user.UserPayments)
                {
                    paymentInfo = payment;
                    break;
                }

                if (paymentInfo == null)
                {
                    paymentInfo = new UserPayment { UserId = user.Id };
                    user.UserPayments.Add(paymentInfo);
                }

                paymentInfo.NameOnCard = model.Payment.NameOnCard;
                paymentInfo.CardNumber = model.Payment.CardNumber;
                paymentInfo.ExpirationDate = model.Payment.ExpirationDate;
                paymentInfo.CVVCode = model.Payment.CVVCode;

 

                if (paymentInfo.PaymentId == 0)
                {
                    _context.UserPayments.Add(paymentInfo);
                }
                else
                {
                    _context.Entry(paymentInfo).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();

                var updatedUser = await _userManager.Users.Include(u => u.UserPayments).FirstOrDefaultAsync(u => u.Id == user.Id);

                if (updatedUser == null)
                {
                    user = updatedUser;
                }

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
