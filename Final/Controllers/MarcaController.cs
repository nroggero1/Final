using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Controllers
{
    public class MarcaController : Controller
    {
        private readonly FinalWebContext _context;
        public MarcaController(FinalWebContext context)
        {
            _context = context;
        }

        // URL: /Marca
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var marcas = await _context.Marca.ToListAsync();
            return View(marcas);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarMarca(int? id)
        {
            if (id == null || _context.Marca == null)
            {
                return NotFound();
            }

            var marca = await _context.Marca.FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // URL: /Marca/CrearMarca
        public IActionResult CrearMarca()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearMarca(Marca marca)
        {
            if (ModelState.IsValid)
            {
                marca.FechaAlta = System.DateTime.Now;
                marca.Activo = true;
                _context.Marca.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }

        // URL: /Marca/EditarMarca
        [HttpGet]
        public async Task<IActionResult> EditarMarca(int? id)
        {
            if (id == null || _context.Marca == null)
            {
                return NotFound();
            }

            var marca = await _context.Marca.FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        [HttpPost]
        public async Task<IActionResult> EditarMarca(int id, Marca marca)
        {
            if (id != marca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var marcaToUpdate = await _context.Marca.FirstOrDefaultAsync(c => c.Id == id);
                    marcaToUpdate.Nombre = marca.Nombre;
                    // Mantener el valor original de FechaAlta
                    marca.FechaAlta = marcaToUpdate.FechaAlta;
                    marcaToUpdate.Activo = marca.Activo;

                    _context.Update(marcaToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(marca.Id))
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

            return View(marca);
        }

        private bool MarcaExists(int id)
        {
            return _context.Marca.Any(c => c.Id == id);
        }
    }
}
