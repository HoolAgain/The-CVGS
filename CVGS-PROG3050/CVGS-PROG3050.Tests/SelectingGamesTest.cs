using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace CVGS_PROG3050.Tests
{
    public class SelectingGamesTest
    {
        private readonly GameController _controller;
        private readonly VaporDbContext _context;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public SelectingGamesTest()
        {
            // Using an in-memory database for testing purposes
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: "TestGameDatabase")
                .Options;

            _context = new VaporDbContext(options);

            // Clear any existing data for testing purposes
            _context.Games.RemoveRange(_context.Games);
            _context.SaveChanges();

            // Seed data
            _context.Games.AddRange(
                new Game { GameId = 1000, Name = "Test1", Genre = "Action" },
                new Game { GameId = 2000, Name = "Test2", Genre = "Role-Playing" }
            );
            _context.SaveChanges();

            _mockUserManager = MockUserManager();
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("test-user-id");

            // Set up controller with in-memory context
            _controller = new GameController(_context, _mockUserManager.Object, null);
        }

        [Fact]
        public async Task AllGamesView_ReturnsAllGames()
        {
            // Act
            var result = await _controller.AllGamesView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<GameViewModel>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void CorrectFilterByGenre()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game {GameId = 1, Name = "Test", Genre = "Action"},
                new Game {GameId = 2, Name = "Test2", Genre = "Role-Playing"}
            }.AsQueryable();

            var filteredGames = games.Where(g => g.Genre == "Action");

            // Act
            var result = filteredGames.ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Test", result.First().Name);
        }

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            return mockUserManager;
        }
    }
}
