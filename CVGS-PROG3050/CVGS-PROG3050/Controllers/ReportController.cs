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

        public ReportController(GameService gameService, UserService userService, EventService eventService)
        {
            _gameService = gameService;
            _userService = userService;
            _eventService = eventService;
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
    }
}
