using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace CVGS_PROG3050.Tests
{
    public class FriendTest
    {
        private readonly FriendController _controller;
        private readonly VaporDbContext _context;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public FriendTest()
        {
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: "TestFriendDatabase")
                .Options;

            _context = new VaporDbContext(options);

            _mockUserManager = MockUserManager();
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("test-user-id");

            _controller = new FriendController(_context, _mockUserManager.Object);

            _controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new Microsoft.AspNetCore.Http.DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>()
            );
        }

        [Fact]
        public async Task AddFriend_SuccessfullyAddsFriend()
        {
            // Arrange
            var friendUser = new User { Id = "new-friend-id", UserName = "NewFriend" };

            // Mock UserManager to find the friend by username
            _mockUserManager.Setup(um => um.FindByNameAsync("NewFriend")).ReturnsAsync(friendUser);
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("test-user-id");

            // Act
            var result = await _controller.AddFriend("NewFriend");

            // Assert
            // Check if the friend was successfully added to the database
            var friendEntry = await _context.Friends
                .FirstOrDefaultAsync(f => f.UserId == "test-user-id" && f.FriendUserId == "new-friend-id");

            Assert.NotNull(friendEntry);
            Assert.Equal("test-user-id", friendEntry.UserId);
            Assert.Equal("new-friend-id", friendEntry.FriendUserId);
        }

        [Fact]
        public async Task RemoveFriend_RemovesFriendSuccessfully()
        {
            // Arrange: Add a friend to remove
            var friendUser = new User { Id = "friend-to-remove-id", UserName = "FriendToRemove" };
            await _context.Users.AddAsync(friendUser);
            await _context.Friends.AddAsync(new Friend { UserId = "test-user-id", FriendUserId = "friend-to-remove-id" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.RemoveFriend("friend-to-remove-id");

            // Assert
            var friends = await _context.Friends.FirstOrDefaultAsync(f => f.UserId == "test-user-id" && f.FriendUserId == "friend-to-remove-id");
            Assert.Null(friends); // Friend should be removed
        }

            private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            return mockUserManager;
        }
    }
}
