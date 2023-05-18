using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;

namespace Final.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly FinalWebContext _context;
        public UsuarioController(FinalWebContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }










        // URL: /Usuario/CrearUsuario
        public IActionResult CrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuarior(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }
    }
}
