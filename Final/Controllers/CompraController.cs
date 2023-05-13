using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class CompraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
