using Final.Data;
using Final.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    public class ClienteController : Controller
    {

        private readonly FinalWebContext _context;
        public ClienteController(FinalWebContext context)
        {
            _context = context;
        }


        // URL Cliente/CrearCliente

            [HttpGet]
            public IActionResult CrearCliente()
            {
                ViewBag.Localidades = _context.Localidad.ToList();
                return View();
            }

            [HttpPost]
            public IActionResult CrearCliente(Cliente cliente)
            {
                if (ModelState.IsValid)
                {
                    // Obtener el Id de la localidad basado en el CodigoPostal
                    int? idLocalidad = _context.Localidad
                        .Where(l => l.CodigoPostal == cliente.CodigoPostal)
                        .Select(l => l.Id)
                        .FirstOrDefault();

                    if (idLocalidad != null)
                    {
                        cliente.IdLocalidad = idLocalidad.Value;
                        _context.Add(cliente);
                        _context.SaveChanges();
                        return RedirectToAction("Index"); // Redirigir a la página deseada después de guardar el cliente
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se encontró una localidad con el código postal especificado.");
                    }
                }

                ViewBag.Localidades = _context.Localidad.ToList();
                return View(cliente);
            }
        }
    }
