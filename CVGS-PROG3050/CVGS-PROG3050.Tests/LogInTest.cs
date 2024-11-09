using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CVGS_PROG3050.Tests
{
    public class LogInTest
    {
        [Fact]
        public async Task CheckForValidLogIn()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            {
                UserName = "Test",
                Password = "Test123!",
                RememberMe = false
            };

            // Creating Mock Objects
            var mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

            var mockSignInManager = new Mock<SignInManager<User>>(
                mockUserManager.Object, new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object, null, null, null, null);

            // Mock valid user
            mockUserManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { UserName = "Test" });

            // Mock successful sign-in
            mockSignInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, true))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new AccountController(null, mockUserManager.Object, mockSignInManager.Object, null);

            // Act
            var result = await controller.LogIn(loginViewModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }
    }
}
