using Final.Data;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    public class ProvinciaController : Controller
    {
        private readonly FinalWebContext _context;
        public ProvinciaController(FinalWebContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
