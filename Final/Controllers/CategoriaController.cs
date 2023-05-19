using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly FinalWebContext _context;
        public CategoriaController(FinalWebContext context)
        {
            _context = context;
        }

        // URL: /Categoria
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categoria.ToListAsync();
            return View(categorias);
        }

        [HttpGet]
        public async Task<IActionResult>
            ConsultarCategoria(int? id)
        {
            if (id == null || _context.Categoria == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
            .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }


        // URL: /Categoria/CrearCategoria
        public IActionResult CrearCategoria()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearCategoria(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                categoria.FechaAlta = System.DateTime.Now;
                categoria.Activo = true;
                _context.Categoria.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }
    }
}
