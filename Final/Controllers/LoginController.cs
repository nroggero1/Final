using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
