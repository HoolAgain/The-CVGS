/* AccountController.cs
* Vapor
* Revision History
* Julia Lebedzeva, 2024.09.28: Created
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

namespace CVGS_PROG3050.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDNTCaptchaValidatorService _captchaValidator;
        private readonly VaporDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;


        public AccountController(VaporDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IDNTCaptchaValidatorService captchaValidator)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _captchaValidator = captchaValidator;

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

            if (!_captchaValidator.HasRequestValidCaptchaEntry())
            {
                ModelState.AddModelError("Captcha", "Captcha is not valid.");
            }


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
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);

                    try
                    {
                        var smtpClient = new SmtpClient("smtp.gmail.com")
                        {
                            Port = 587,
                            Credentials = new NetworkCredential("vapormarketplace@gmail.com", "hiytpdbbojkvcggr"),
                            EnableSsl = true,
                        };

                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress("Vapormarketplace@gmail.com", "Vapor Marketplace"),
                            Subject = "Please verify your email",
                            Body = $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Verify Email</a>",
                            IsBodyHtml = true
                        };

                        mailMessage.To.Add(user.Email);

                        await smtpClient.SendMailAsync(mailMessage);
                        return RedirectToAction("EmailVerificationSent");
                        
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"There was an error sending the confirmation email");
                    }
                

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("EmailVerificationSent");
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

        [HttpGet]
        public IActionResult EmailVerificationSent()
        {
            return View();
        }

        [HttpGet]
        public async Task <IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");  
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            return View("Error");
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
                        return View("loginview", model);
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");

            }
            return View("loginview",model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Email is required.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("vapormarketplace@gmail.com", "Wearethebest"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("vapormarketplace@gmail.com", "Vapor Marketplace"),
                    Subject = "Password Reset",
                    Body = $"Reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(user.Email);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "There was an error sending the password reset email.");
            }

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return View("Error");
            }
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation() => View();

        // Logic for profile
        [HttpGet]
        public async Task<IActionResult> Profile(string activeTab = "Profile")
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
            var allUsers = await _userManager.Users
                .Where(u => u.Id != userId)
                .Select(u => new FriendViewModel
                 {
                     UserId = u.Id,
                     Username = u.UserName
                 }).ToListAsync();

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

            var friendsList = await GetFriendsListAsync(userId);
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
                },
                Friends = friendsList,
                AllUsers = allUsers
            };
            System.Diagnostics.Debug.WriteLine($"Profile Loaded: Payment Info - NameOnCard = {model.Payment.NameOnCard}, CardNumber = {model.Payment.CardNumber}, ExpirationDate = {model.Payment.ExpirationDate}, CVVCode = {model.Payment.CVVCode}");

            ViewData["ActiveTab"] = activeTab;
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
                    TempData["ProfileStatus"] = "Profile updated successfully.";
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                    TempData["ProfileStatus"] = "An error occurred while updating the profile.";
                }

            }

            model.Email = user.Email;
            model.UserName = user.UserName;
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
                    TempData["PreferenceStatus"] = "Preferences updated successfully.";
                    return RedirectToAction("Profile", "Account", new {activeTab = "Preferences"});
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error updating user: {item.Description}");
                    }
                    TempData["PreferenceStatus"] = "An error occurred while updating preferences.";
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
                TempData["PreferenceStatus"] = "There was an error with your submission.";
            }

            model.Preferences.FavoriteCategory = user.FavoriteCategory;
            model.Preferences.FavoritePlatform = user.FavoritePlatform;
            model.Preferences.LanguagePreference = user.LanguagePreference;
            ViewData["ActiveTab"] = "Preferences";
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

                try
                {
                    await _context.SaveChangesAsync();

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        TempData["AddressStatus"] = "Shipping information saved successfully.";
                        return RedirectToAction("Profile", "Account", new {activeTab = "ShippingInfo"});
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        TempData["AddressStatus"] = "An error occurred while updating the user information.";
                    }
                }
                catch (Exception)
                {
                    TempData["AddressStatus"] = "An error occurred while saving the shipping information.";
                }
                
            }

            model = await PopulateUserProfile(user);
            ViewData["ActiveTab"] = "ShippingInfo";
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


            if (!ModelState.IsValid)
            {
                model = await PopulateUserProfile(user);
                ViewData["ActiveTab"] = "PaymentInfo";
                return View("profileview", model);
            }

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
                    TempData["PaymentStatus"] = "Card added successfully.";
                }
                else
                {
                    _context.Entry(paymentInfo).State = EntityState.Modified;
                    TempData["PaymentStatus"] = "Card updated successfully.";
                }

                try { 
                    await _context.SaveChangesAsync(); TempData["PaymentStatus"] = "Card information saved successfully."; 
                }
                catch (Exception) { 
                    TempData["PaymentStatus"] = "An error occurred while saving the card information."; 
                }

                var updatedUser = await _userManager.Users.Include(u => u.UserPayments).FirstOrDefaultAsync(u => u.Id == user.Id);

                if (updatedUser == null)
                {
                    user = updatedUser;
                }

                return RedirectToAction("Profile", "Account", new {activeTab = "PaymentInfo"});
            }


           

            if (user.UserPayments != null && user.UserPayments.Any())
            {
                var currentPaymentInfo = user.UserPayments.FirstOrDefault();
                if (currentPaymentInfo != null)
                {
                    model.Payment.NameOnCard = currentPaymentInfo.NameOnCard;
                    model.Payment.CardNumber = currentPaymentInfo.CardNumber;
                    model.Payment.CVVCode = currentPaymentInfo.CVVCode;
                    model.Payment.ExpirationDate = currentPaymentInfo.ExpirationDate;
                }
            }
            else
            {
                model.Payment = new PaymentViewModel();
            }


            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"ModelState Error: {state.Key} - {error.ErrorMessage}");
                }
            }

            ViewData["ActiveTab"] = "PaymentInfo";
            return View("profileview", model);
        }

        private async Task<ProfileViewModel> PopulateUserProfile(User user)
        {
            user = await _userManager.Users
                .Include(u => u.Addresses)
                .Include(u => u.UserPayments)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (user == null)
            {
                return null;
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
            return model;
        }

        [HttpGet]
        public async Task<List<FriendViewModel>> GetFriendsListAsync(string userId)
        {
            var friends = await _context.Friends
                .Where(f => f.UserId == userId || f.FriendUserId == userId)
                .Select(f => f.UserId == userId ? f.FriendUser : f.User)
                .Select(f => new FriendViewModel
                {
                    UserId = f.Id,
                    Username = f.UserName
                })
                .ToListAsync();
            return friends;
        }

        [HttpGet]
        public async Task<IActionResult> FriendsWishlist(string friendId)
        {
            var friend = await _userManager.FindByIdAsync(friendId);
            if (friend == null)
            {
                TempData["FriendStatus"] = "Friend doesn't exist";
            }

            var games = await _context.Wishlist
                .Where(w => w.UserId == friendId)
                .Include(w => w.Game)
                .Select(g => new GameViewModel
            {
                GameId = g.Game.GameId,
                Name = g.Game.Name,
                Genre = g.Game.Genre,
                Description = g.Game.Description,
                ReleaseDate = g.Game.ReleaseDate,
                Developer = g.Game.Developer,
                Publisher = g.Game.Publisher,
                Price = g.Game.Price
            }).ToListAsync();

            ViewBag.FriendName = friend.UserName;
            ViewBag.FriendId = friendId;
            return View("~/Views/Game/ViewWishlist.cshtml", games);
        }
    }
}
