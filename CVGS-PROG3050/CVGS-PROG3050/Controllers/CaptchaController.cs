using Microsoft.AspNetCore.Mvc;

namespace CVGS_PROG3050.Controllers
{
    public class CaptchaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
