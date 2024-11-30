using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CVGS_PROG3050.DataAccess;
using System.Linq;
using System.Web;
using CVGS_PROG3050.Entities;
using CVGS_PROG3050.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DNTCaptcha.Core;
using CaptchaMvc.Attributes;
using CaptchaMvc.Infrastructure;
using CaptchaMvc.HtmlHelpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace CVGS_PROG3050.Controllers
{

    [Authorize]
    public class EventsController : Controller
    {
        private readonly VaporDbContext _context;
        private readonly UserManager<User> _userManager;

        public EventsController(VaporDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> EventsView()
        {
            var events = await _context.Events
                .Include(e => e.UserEvents)
                .Select(e => new EventViewModel
                {
                    Id = e.EventId,
                    EventName = e.EventName,
                    EventDate = e.EventDate,
                    EventPrice = e.EventPrice,
                    Location = e.Location,
                    LocationType = e.LocationType,
                    Description = e.Description,
                    UserEvents = e.UserEvents,
                    CurrentUserId = _userManager.GetUserId(User)
                }).ToListAsync();


           

            return View("EventsView", events ?? new List<EventViewModel>());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View("CreateEvent");
        }
        [HttpPost]
        public async Task<IActionResult> Create (Event model)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                TempData["EventStatus"] = "Event created successfully.";
                return RedirectToAction("EventsView");
            }
            else
            {
                TempData["EventStatus"] = "There was an error with your submission.";
            }
            return View("CreateEvent", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AdminPanelViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                _context.Events.Add(model.Event);
                await _context.SaveChangesAsync();
                TempData["EventStatus"] = "Event created successfully.";
                return RedirectToAction("EventsView");
            }
            else
            {
                TempData["EventStatus"] = "There was an error with your submission.";
            }
            return View("CreateEvent", model); 
        }

        [HttpPost]
        public async Task<IActionResult> Register(int eventId)
        {
            var userId = _userManager.GetUserId(User);
            var registration = new UserEvent 
            { 
                UserId = userId, 
                EventId = eventId 
            };

            _context.UserEvents.Add(registration);
            await _context.SaveChangesAsync();

            return RedirectToAction("EventsView");
        }

        [HttpPost]
        public async Task<IActionResult> unRegister(int eventId)
        {
            var userId = _userManager.GetUserId(User);
            var registration = await _context.UserEvents.FirstOrDefaultAsync(ue => ue.EventId == eventId && ue.UserId == userId);

            if (registration != null)
            {
                _context.UserEvents.Remove(registration);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EventsView");
        }

    }
}
