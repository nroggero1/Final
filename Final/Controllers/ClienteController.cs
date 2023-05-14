using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;

namespace Final.Controllers
{
    public class ClienteController : Controller
    {
        private readonly FinalWebContext _context;
        public ClienteController(FinalWebContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
