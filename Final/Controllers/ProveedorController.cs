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
        [HttpGet]
        public IActionResult CrearProveedor()
        {
            ViewBag.Localidades = _context.Localidad.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CrearProveedore(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                // Obtener el Id de la localidad basado en el CodigoPostal
                int? idLocalidad = _context.Localidad
                    .Where(l => l.CodigoPostal == proveedor.CodigoPostal)
                    .Select(l => l.Id)
                    .FirstOrDefault();

                if (idLocalidad != null)
                {
                    proveedor.IdLocalidad = idLocalidad.Value;
                    _context.Add(proveedor);
                    _context.SaveChanges();
                    return RedirectToAction("Index"); // Redirigir a la página deseada después de guardar el cliente
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se encontró una localidad con el código postal especificado.");
                }
            }

            ViewBag.Localidades = _context.Localidad.ToList();
            return View(proveedor);
        }
    }
    }
