using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
        public async Task<IActionResult> ConsultarProducto(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // URL: /Producto/CrearProducto
        [HttpPost]
        public IActionResult CrearProducto()
        {
            var categorias = _context.Categoria.ToList();
            ViewBag.Categoria = categorias;

            var marcas = _context.Marca.ToList();
            ViewBag.Marca = marcas;

            return View();
        }

        public async Task<IActionResult> CrearProducto(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var marcas = _context.Marca.ToList();
                var categorias = _context.Categoria.ToList();

                ViewBag.Marcas = marcas;
                ViewBag.Categorias = categorias;

                producto.FechaAlta = System.DateTime.Now;
                producto.Activo = true;
                _context.Producto.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }
    }
}