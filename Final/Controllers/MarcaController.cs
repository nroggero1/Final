using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class MarcaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
