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
        public async Task<IActionResult> ConsultarCategoria(int? id)
        {
            if (id == null || _context.Categoria == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.Id == id);
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

        // URL: /Categoria/EditarCategoria
        [HttpGet]
        public async Task<IActionResult> EditarCategoria(int? id)
        {
            if (id == null || _context.Categoria == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoriaToUpdate = await _context.Categoria.FirstOrDefaultAsync(c => c.Id == id);
                    categoriaToUpdate.Nombre = categoria.Nombre;
                    // Mantener el valor original de FechaAlta
                    categoria.FechaAlta = categoriaToUpdate.FechaAlta;
                    categoriaToUpdate.Activo = categoria.Activo;

                    _context.Update(categoriaToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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

            return View(categoria);
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(c => c.Id == id);
        }
    }
}
