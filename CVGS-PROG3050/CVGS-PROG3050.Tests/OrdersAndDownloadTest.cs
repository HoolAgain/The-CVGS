using System.Security.Claims;
using System.Text;
using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CVGS_PROG3050.Tests
{
    public class OrdersAndDownloadTest
    {
        private readonly VaporDbContext _context;
        private readonly OrderController _controller;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public OrdersAndDownloadTest()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new VaporDbContext(options);

            // Mock UserManager
            _mockUserManager = MockUserManager();
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("test-user-id");

            // Initialize controller
            _controller = new OrderController(_context, _mockUserManager.Object);
        }

        [Fact]
        public async Task PurchasedGames_ReturnsCorrectGames()
        {
            // Arrange
            var userId = "test-user-id";
            var testUser = new User { Id = userId, UserName = "testuser" };
            var testGame = new Game { GameId = 1, Name = "Test Game" };
            var testOrder = new Order
            {
                OrderId = 1,
                UserId = userId,
                Game = testGame,
                OrderDate = DateTime.Now
            };

            _context.Users.Add(testUser);
            _context.Games.Add(testGame);
            _context.Orders.Add(testOrder);
            await _context.SaveChangesAsync();

            // Mock the logged-in user
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(testUser);

            MockLoggedInUser(userId);

            // Act
            var result = await _controller.PurchasedGames();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<PurchasedGameViewModel>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal("Test Game", model.First().GameName);
        }

        [Fact]
        public void DownloadGameFile_ReturnsCorrectFile()
        {
            // Arrange
            var gameName = "Test Game";

            // Act
            var result = _controller.DownloadGameFile(gameName);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("text/plain", fileResult.ContentType);
            Assert.Equal("Test_Game.txt", fileResult.FileDownloadName);
            Assert.Contains("Thank you for purchasing Test Game!", Encoding.UTF8.GetString(fileResult.FileContents));
        }

        [Fact]
        public void UpdateOrderStatus_ChangesStatusToProcessed()
        {
            // Arrange
            var testOrder = new Order
            {
                OrderId = 1,
                Status = "Pending"
            };

            _context.Orders.Add(testOrder);
            _context.SaveChanges();

            // Act
            var result = _controller.UpdateOrderStatus(testOrder.OrderId);

            // Assert
            var updatedOrder = _context.Orders.FirstOrDefault(o => o.OrderId == testOrder.OrderId);
            Assert.NotNull(updatedOrder);
            Assert.Equal("Processed", updatedOrder.Status);
            Assert.IsType<RedirectToActionResult>(result);
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

