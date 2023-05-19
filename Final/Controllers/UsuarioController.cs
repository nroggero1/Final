using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly FinalWebContext _context;
        public UsuarioController(FinalWebContext context)
        {
            _context = context;
        }

        // URL: /Usuario
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuario.ToListAsync();
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult>
            ConsultarUsuario(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
            .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
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
