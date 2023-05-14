using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;

namespace Final.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly FinalWebContext _context;
        public CategoriaController(FinalWebContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
