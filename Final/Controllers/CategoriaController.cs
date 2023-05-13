using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class CategoriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
