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
        public async Task<IActionResult> ConsultarUsuario(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == id);
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
        public async Task<IActionResult> CrearUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }


        // URL: /Usuario/EditarUsuario
        [HttpGet]
        public async Task<IActionResult> EditarUsuario(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioToUpdate = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == id);
                    usuarioToUpdate.NombreUsuario = usuario.NombreUsuario;
                    usuarioToUpdate.Clave = usuario.Clave;
                    usuarioToUpdate.Nombre = usuario.Nombre;
                    usuarioToUpdate.Apellido = usuario.Apellido;
                    usuarioToUpdate.Mail = usuario.Mail;
                    usuarioToUpdate.FechaNacimiento = usuario.FechaNacimiento;
                    // Mantener el valor original de FechaAlta
                    usuario.FechaAlta = usuarioToUpdate.FechaAlta;
                    usuarioToUpdate.Administrador = usuario.Administrador;
                    usuarioToUpdate.Activo = usuario.Activo;

                    _context.Update(usuarioToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(u => u.Id == id);
        }


    }
}
