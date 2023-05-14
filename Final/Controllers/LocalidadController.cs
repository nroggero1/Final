using Final.Data;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    public class LocalidadController : Controller
    {
        private readonly FinalWebContext _context;
        public LocalidadController(FinalWebContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
