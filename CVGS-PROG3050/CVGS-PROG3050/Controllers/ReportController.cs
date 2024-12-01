using Microsoft.AspNetCore.Mvc;
using CVGS_PROG3050.Services;
using CVGS_PROG3050.Entities;

namespace CVGS_PROG3050.Controllers
{
    public class ReportController : Controller
    {
        private readonly GameService _gameService;
        private readonly UserService _userService;
        private readonly EventService _eventService;
        private readonly OrderService _orderService;
        private readonly WishlistService _wishlistService;
        private readonly ReviewService _reviewService;

        public ReportController(GameService gameService, UserService userService, EventService eventService, 
            OrderService orderService, WishlistService wishlistService, ReviewService reviewService)
        {
            _gameService = gameService;
            _userService = userService;
            _eventService = eventService;
            _orderService = orderService;
            _wishlistService = wishlistService;
            _reviewService = reviewService;
        }

        [HttpGet("GameReport/ExportGamesToExcel")]
        public IActionResult ExportGamesToExcel()
        {
            // Retrieve all game data
            List<Game> games = _gameService.GetAllGames();

            // Generate Excel file
            byte[] fileContent = _gameService.GenerateGameReportExcel(games);

            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GameReport.xlsx");
        }

        [HttpGet("UserReport/ExportMembersToExcel")]
        public IActionResult ExportMembersToExcel()
        {
            List<User> users = _userService.GetAllMembers();

            byte[] fileContent = _userService.GenerateUserReportExcel(users);

            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UserReport.xlsx");
        }

        [HttpGet("EventReport/ExportEventsToExcel")]
        public IActionResult ExportEventsToExcel()
        {
            var events = _eventService.GetEventReportData();

            var fileContent = _eventService.GenerateEventReportExcel(events);
 
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EventReport.xlsx");
        }

        [HttpGet("OrderReport/ExportOrdersToExcel")]
        public IActionResult ExportOrdersToExcel()
        {
            var orders = _orderService.GetOrderReportData();

            var fileContent = _orderService.GenerateOrderReportExcel(orders);

            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderReport.xlsx");
        }

        [HttpGet("WishlistReport/ExportWishlistToExcel")]
        public IActionResult ExportWishlistToExcel()
        {
            var wishlist = _wishlistService.GetWishlistReportData();

            var fileContent = _wishlistService.GenerateWishlistReportExcel(wishlist);

            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WishlistReport.xlsx");
        }

        [HttpGet("ReviewReport/ExportReviewsToExcel")]
        public IActionResult ExportReviewsToExcel()
        {
            var reviews = _reviewService.GetReviewReportData();

            var fileContent = _reviewService.GenerateReviewReportExcel(reviews);

            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReviewReport.xlsx");
        }
    }
}
