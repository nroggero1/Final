using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class ProveedorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
