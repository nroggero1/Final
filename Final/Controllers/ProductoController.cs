using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Controllers
{
    public class ProductoController : Controller
    {
        private readonly FinalWebContext _context;
        public ProductoController(FinalWebContext context)
        {
            _context = context;
        }


        // URL: /Producto
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Producto.ToListAsync();
            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult>
            ConsultarProducto(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
            .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }



        // URL: /Producto/CrearProducto

        public IActionResult CrearProducto()
        {
            var marcas = _context.Marca.ToList();
            var categorias = _context.Categoria.ToList();

            ViewBag.Marcas = marcas;
            ViewBag.Categorias = categorias;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto(Producto producto)
        {
            if (ModelState.IsValid)
            {
                // Cálculo del precio de venta sugerido
                producto.PrecioVentaSugerido = producto.PrecioCompra * producto.PorcentajeGanacia / 100;

                producto.Activo = true;
                producto.FechaAlta = DateTime.Now;

                _context.Producto.Add(producto);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Recarga las marcas y categorías en caso de error de validación
            var marcas = _context.Marca.ToList();
            var categorias = _context.Categoria.ToList();

            ViewBag.Marcas = marcas;
            ViewBag.Categorias = categorias;

            return View(producto);
        }
    }
}
