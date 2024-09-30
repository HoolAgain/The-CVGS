using Microsoft.AspNetCore.Mvc;

namespace CVGS_PROG3050.Controllers
{
    public class PasswordReset : Controller
    {
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

    }
}
