using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class VentaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
