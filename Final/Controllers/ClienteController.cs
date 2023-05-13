using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
