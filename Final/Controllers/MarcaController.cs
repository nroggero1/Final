using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;

namespace Final.Controllers
{
    public class MarcaController : Controller
    {
        private readonly FinalWebContext _context;
        public MarcaController(FinalWebContext context)
        {
            _context = context;
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
                _context.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }
    }
}
