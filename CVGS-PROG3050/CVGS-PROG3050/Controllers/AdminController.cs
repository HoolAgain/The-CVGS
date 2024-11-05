using Microsoft.AspNetCore.Mvc;

namespace CVGS_PROG3050.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminPanelView()
        {
            return View();
        }
    }
}
