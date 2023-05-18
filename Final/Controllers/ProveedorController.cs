using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;

namespace Final.Controllers
{
    public class ProveedorController : Controller
    {

            private readonly FinalWebContext _context;
            public ProveedorController(FinalWebContext context)
            {
                _context = context;
            }











            // URL: /Proveedor/CrearProveedor
            public IActionResult CrearProveedor()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> CrearProveedor(Proveedor proveedor)
            {
                if (ModelState.IsValid)
                {
                    _context.Proveedor.Add(proveedor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(proveedor);
            }
        }
    }
