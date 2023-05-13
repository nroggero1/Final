using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
