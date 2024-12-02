using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CVGS_PROG3050.Tests
{
    public class RatingAndReviewTest
    {
        private VaporDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new VaporDbContext(options);
        }

        private UserManager<User> GetMockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null).Object;
        }

        private HomeController CreateHomeControllerWithDependencies(VaporDbContext context)
        {
            var mockLogger = new Mock<ILogger<HomeController>>();
            var mockUserManager = GetMockUserManager();
            return new HomeController(mockLogger.Object, context, mockUserManager);
        }

        [Fact]
        public async Task Index_ShouldHandleNoReviewsOrRatings()
        {
            // Arrange: Create an isolated in-memory database
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VaporDbContext(options);
            var controller = CreateHomeControllerWithDependencies(context);

            // Act
            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<GameViewModel>>(viewResult.Model);

            // Assert
            Assert.Empty(model);
        }

        [Fact]
        public async Task Index_ShouldReturnGamesWithRatingsAndReviews()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            // Seed data
            var game = new Game { GameId = 1, Name = "Test Game" };
            var review = new Review { ReviewId = 1, GameId = 1, ReviewText = "Great game!", UserId = "user1" };
            var rating = new Rating { RatingId = 1, GameId = 1, Score = 5, UserId = "user1" };

            context.Games.Add(game);
            context.Reviews.Add(review);
            context.Ratings.Add(rating);
            await context.SaveChangesAsync();

            var controller = CreateHomeControllerWithDependencies(context);

            // Act
            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<GameViewModel>>(viewResult.Model);

            // Assert
            Assert.Single(model); // One game
            Assert.Equal("Test Game", model[0].Name);
            Assert.Equal("5.0", model[0].AverageRating);
            Assert.Equal("Great game!", model[0].RandomReview);
        }
    }
}
