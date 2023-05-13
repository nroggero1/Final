using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class Usuario_Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
