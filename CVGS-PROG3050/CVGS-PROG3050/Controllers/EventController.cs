using Microsoft.AspNetCore.Mvc;

namespace CVGS_PROG3050.Controllers
{
    public class EventController : Controller
    {
        public IActionResult EventView()
        {
            return View();
        }
    }
}
