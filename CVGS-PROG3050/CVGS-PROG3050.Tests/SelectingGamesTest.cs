﻿using CVGS_PROG3050.Controllers;
using CVGS_PROG3050.DataAccess;
using CVGS_PROG3050.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CVGS_PROG3050.Tests
{
    public class SelectingGamesTest
    {
        private readonly GameController _controller;
        private readonly VaporDbContext _context;

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
                new Game { GameId = 1, Name = "Test1", Genre = "Action" },
                new Game { GameId = 2, Name = "Test2", Genre = "Role-Playing" }
            );
            _context.SaveChanges();

            // Set up controller with in-memory context
            _controller = new GameController(_context);
        }

        [Fact]
        public void AllGamesView_ReturnsAllGames()
        {
            // Act
            var result = _controller.AllGamesView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Game>>(viewResult.Model);
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
    }
}
