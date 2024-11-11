using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace CVGS_PROG3050.Tests
{
    public class EventTest
    {
        private readonly EventsController _controller;
        private readonly VaporDbContext _context;
        private readonly Mock<UserManager<User>> _mockUserManager;

        public EventTest()
        {
            // Using an in-memory database for testing purposes
            var options = new DbContextOptionsBuilder<VaporDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new VaporDbContext(options);

            // Clear existing data for a clean test environment
            _context.Events.RemoveRange(_context.Events);
            _context.UserEvents.RemoveRange(_context.UserEvents);
            _context.SaveChanges();

            // Add sample event data
            _context.Events.Add(new Event
            {
                EventId = 1,
                EventName = "Gaming Expo",
                EventDate = DateTime.Now,
                Location = "Convention Center",
                LocationType = "Indoor",
                Description = "A fun gaming event",
                EventPrice = 15.00m
            });
            _context.SaveChanges();

            // Set up UserManager mock
            _mockUserManager = MockUserManager();
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("test-user-id");

            // Initialize controller with mock UserManager and in-memory DbContext
            _controller = new EventsController(_context, _mockUserManager.Object);
        }

        [Fact]
        public async Task Register_AddsUserEventEntry()
        {
            // Act
            var result = await _controller.Register(1);

            // Assert
            var userEvent = _context.UserEvents.FirstOrDefault(ue => ue.EventId == 1 && ue.UserId == "test-user-id");
            Assert.NotNull(userEvent);
            Assert.Equal("test-user-id", userEvent.UserId);
            Assert.Equal(1, userEvent.EventId);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("EventsView", redirectResult.ActionName);
        }

        [Fact]
        public async Task Unregister_RemovesUserEventEntry()
        {
            // Arrange: Add an existing UserEvent for the test user
            _context.UserEvents.Add(new UserEvent
            {
                UserId = "test-user-id",
                EventId = 1
            });
            _context.SaveChanges();

            // Act
            var result = await _controller.unRegister(1);

            // Assert
            var userEvent = _context.UserEvents.FirstOrDefault(ue => ue.EventId == 1 && ue.UserId == "test-user-id");
            Assert.Null(userEvent);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("EventsView", redirectResult.ActionName);
        }

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            return mockUserManager;
        }
    }
}
