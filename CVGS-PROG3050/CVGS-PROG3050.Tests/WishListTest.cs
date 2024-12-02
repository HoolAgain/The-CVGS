using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace CVGS_PROG3050.Tests
{
    public class WishListTest
    {
        private readonly GameController _controller;
        private readonly VaporDbContext _context;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public WishListTest()
        {
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: "TestWishlistDatabase")
                .Options;
                _context = new VaporDbContext(options);

            _context.Games.RemoveRange(_context.Games);
            _context.Wishlist.RemoveRange(_context.Wishlist);
            _context.SaveChanges();

            _context.Games.Add(new Game { GameId = 1, Name = "Test Game", Genre = "Action" });
            _context.SaveChanges();

            _mockUserManager = MockUserManager();
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("test-user-id");

            _controller = new GameController(_context, _mockUserManager.Object, null);

            _controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new Microsoft.AspNetCore.Http.DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
            );
        }

        [Fact]
        public async Task AddGameToWishList()
        {
            // Act
            var result = await _controller.AddToWishlist(1);

            // Assert
            var wishlistItem = await _context.Wishlist.FirstOrDefaultAsync(w => w.GameId == 1 && w.UserId == "test-user-id");
            Assert.NotNull(wishlistItem);
            Assert.Equal("test-user-id", wishlistItem.UserId);
            Assert.Equal(1, wishlistItem.GameId);
        }

        [Fact]
        public async Task RemoveGameFromWishList()
        {
            // Arrange
            _context.Wishlist.Add(new Wishlist { GameId = 1, UserId = "test-user-id" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.RemoveFromWishlist(1);

            // Assert
            var wishlistItem = await _context.Wishlist.FirstOrDefaultAsync(w => w.GameId == 1 && w.UserId == "test-user-id");
            Assert.Null(wishlistItem);
        }

        [Fact]
        public async Task ShowGamesInWishList()
        {
            // Arrange
            _context.Games.AddRange(
                new Game { GameId = 4, Name = "Test Game 1", Genre = "Action" },
                new Game { GameId = 5, Name = "Test Game 2", Genre = "Adventure" },
                new Game { GameId = 6, Name = "Test Game 3", Genre = "Role-Playing" }
             );
            await _context.SaveChangesAsync();

            _context.Wishlist.AddRange(
                new Wishlist { GameId = 4, UserId = "test-user-id" },
                new Wishlist { GameId = 5, UserId = "test-user-id" }
             );
            await _context.SaveChangesAsync();

            // Act
            var result = _controller.ViewWishlist();

            // Assert
            Assert.NotNull(result);
        }


        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
