using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace CVGS_PROG3050.Tests
{
    public class CartTest
    {
        private readonly VaporDbContext _context;
        private readonly CartController _controller;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public CartTest()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
                .Options;
            _context = new VaporDbContext(options);

            // Mock UserManager
            _mockUserManager = MockUserManager();
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("test-user-id");

            // Initialize controller
            _controller = new CartController(_context, _mockUserManager.Object, Mock.Of<ILogger<GameController>>());

            // Setup TempData for notifications
            _controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
            );
        }

        [Fact]
        public async Task AddGamesToCart()
        {
            // Arrange
            var gameId = 1;
            _context.Users.Add(new User { Id = "test-user-id" });
            _context.Games.Add(new Game { GameId = gameId });
            await _context.SaveChangesAsync();
            MockLoggedInUser("test-user-id");

            // Act
            await _controller.AddToCart(gameId);

            // Assert
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.GameId == gameId);
            Assert.NotNull(cartItem);

        }

        [Fact]
        public async Task RemovesGameFromCart()
        {
            // Arrange
            var gameId = 1;
            _context.Users.Add(new User { Id = "test-user-id" });
            _context.Games.Add(new Game { GameId = gameId });
            _context.Carts.Add(new Cart { UserId = "test-user-id", GameId = gameId });
            await _context.SaveChangesAsync();
            MockLoggedInUser("test-user-id");

            // Act
            await _controller.RemoveFromCart(gameId);

            // Assert
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.GameId == gameId);
            Assert.Null(cartItem);
        }

        [Fact]
        public async Task ReturnsCartItems()
        {
            // Arrange
            _context.Users.Add(new User { Id = "test-user-id" });
            _context.Games.Add(new Game { GameId = 1, Name = "Game 1", Price = 29.99m });
            _context.Carts.Add(new Cart { UserId = "test-user-id", GameId = 1 });
            await _context.SaveChangesAsync();
            MockLoggedInUser("test-user-id");

            // Act
            var result = await _controller.ViewCart();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<CartViewModel>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal(1, model.First().GameId);
        }

        [Fact]
        public async Task CreatesOrderAndClearsCart()
        {
            // Arrange
            _context.Users.Add(new User { Id = "test-user-id" });
            _context.Games.Add(new Game { GameId = 1, Price = 29.99m });
            _context.Carts.Add(new Cart { UserId = "test-user-id", GameId = 1 });
            _context.UserPayments.Add(new UserPayment { PaymentId = 1, UserId = "test-user-id" });
            await _context.SaveChangesAsync();
            MockLoggedInUser("test-user-id");

            // Act
            await _controller.Checkout(1);

            // Assert
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.GameId == 1);
            Assert.Null(cartItem);
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.GameId == 1);
            Assert.NotNull(order);
        }

        private void MockLoggedInUser(string userId)
        {
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            return mockUserManager;
        }
    }
}
