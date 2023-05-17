using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;

namespace Final.Controllers
{
    public class ClienteController : Controller
    {

            private readonly FinalWebContext _context;
            public ClienteController(FinalWebContext context)
            {
                _context = context;
            }











            // URL: /Cliente/CrearCliente
            public IActionResult CrearCliente()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> CrearCliente(Cliente cliente)
            {
                if (ModelState.IsValid)
                {
                    _context.Cliente.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(cliente);
            }
        }
    }
