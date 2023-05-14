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
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }
    }
}
