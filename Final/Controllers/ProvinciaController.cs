using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;
using Microsoft.EntityFrameworkCore;

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
