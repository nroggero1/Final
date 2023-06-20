using Final.Data;
using Final.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult CrearProducto()
        {
            var categorias = _context.Categoria.ToList();
            ViewBag.Categorias = categorias;

            var marcas = _context.Marca.ToList();
            ViewBag.Marcas = marcas;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var categorias = _context.Categoria.ToList();
                ViewBag.Categorias = categorias;

                var marcas = _context.Marca.ToList();
                ViewBag.Marcas = marcas;

                producto.FechaAlta = DateTime.Now;
                producto.Activo = true;

                _context.Producto.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        // URL: /Producto/EditarProducto
        [HttpGet]
        public async Task<IActionResult> EditarProducto(int? id)
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

            var categorias = _context.Categoria.ToList();
            ViewBag.Categorias = categorias;

            var marcas = _context.Marca.ToList();
            ViewBag.Marcas = marcas;

            producto.PrecioVentaSugerido = (producto.PorcentajeGanancia / 100) * producto.PrecioCompra + producto.PrecioCompra;

            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productoToUpdate = await _context.Producto.FirstOrDefaultAsync(p => p.Id == id);
                    productoToUpdate.Nombre = producto.Nombre;
                    productoToUpdate.Descripcion = producto.Descripcion;
                    productoToUpdate.CodigoBarras = producto.CodigoBarras;
                    productoToUpdate.PrecioCompra = producto.PrecioCompra;
                    productoToUpdate.PorcentajeGanancia = producto.PorcentajeGanancia;
                    productoToUpdate.PrecioVentaSugerido = producto.PrecioVentaSugerido;
                    productoToUpdate.PrecioVenta = producto.PrecioVenta;
                    productoToUpdate.Stock = producto.Stock;
                    productoToUpdate.StockMinimo = producto.StockMinimo;
                    productoToUpdate.Activo = producto.Activo;
                    productoToUpdate.IdMarca = producto.IdMarca;
                    productoToUpdate.IdCategoria = producto.IdCategoria;
                    // Mantener el valor original de FechaAlta
                    producto.FechaAlta = productoToUpdate.FechaAlta;

                    _context.Update(productoToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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

            var categorias = _context.Categoria.ToList();
            ViewBag.Categorias = categorias;

            var marcas = _context.Marca.ToList();
            ViewBag.Marcas = marcas;

            return View(producto);
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(p => p.Id == id);
        }


        // URL: /Producto/ConsultarBajoStock
        [HttpGet]
        public async Task<IActionResult> ConsultarProductosBajoStock()
        {
            var productosBajoStock = _context.Producto.Where(p => p.Stock <= p.StockMinimo && p.Activo).ToList();
            return View(productosBajoStock);
        }
    }
}