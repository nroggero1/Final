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
        public async Task<IActionResult>
            ConsultarCategoria(int? id)
        {
            if (id == null || _context.Marca == null)
            {
                return NotFound();
            }

            var marca = await _context.Marca
            .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Marca.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }
    }
}
