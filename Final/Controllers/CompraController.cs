using Final.Data;
using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Final.Controllers
{
    public class CompraController : Controller
    {
        private readonly FinalWebContext _context;
        public CompraController(FinalWebContext context)
        {
            _context = context;
        }

        // URL: /Compra
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var compras = await _context.Compra.ToListAsync();
            return View(compras);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarCompra(int? id)
        {
            if (id == null || _context.Compra == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FirstOrDefaultAsync(c => c.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            var detallesCompra = await _context.DetalleCompra.Where(c => c.IdCompra == id).ToListAsync();
            var compraViewModel = new CompraViewModel()
            {
                IdProveedor = compra.IdProveedor,
                Fecha = compra.Fecha,
                Importe = compra.Importe
            };

            var detallesCompraViewModel = new List<DetalleCompraViewModel>();
            foreach (var detalleCompra in detallesCompra)
            {
                var detalleCompraViewModel = new DetalleCompraViewModel()
                {
                    IdVenta = detalleCompra.IdCompra,
                    IdProducto = detalleCompra.IdProducto,
                    Cantidad = detalleCompra.Cantidad,
                    PrecioUnitario = detalleCompra.PrecioUnitario
                };

                detallesCompraViewModel.Add(detalleCompraViewModel);

            }

            compraViewModel.DetallesCompra = detallesCompraViewModel;

            return View(compraViewModel);
        }


        // URL: /compra/RegistrarCompra
        [HttpGet]
        public Task<IActionResult> RegistrarCompra()
        {
            var marcas = _context.Marca.ToList();
            var categorias = _context.Categoria.ToList();
            var productos = _context.Producto.ToList();

            var listProductos = new Dictionary<int, string>();
            foreach (var x in productos)
            {
                var nombreCategoria = categorias.FirstOrDefault(y => y.Id == x.IdCategoria)?.Nombre;
                var nombreMarca = marcas.FirstOrDefault(y => y.Id == x.IdMarca)?.Nombre;
                var nombreCategoriaYmarca = $"codBar: {x.CodigoBarras} Cod: {x.Id} -> {nombreCategoria} -> {nombreMarca} -> {x.Nombre}";
                listProductos.Add(x.Id, nombreCategoriaYmarca);
            }

            ViewBag.Productos = listProductos;

            var proveedores = _context.Proveedor.ToList();
            ViewBag.Proveedores = proveedores;

            ViewBag.ImporteTotal = 0;

            return Task.FromResult<IActionResult>(View());
        }

        // URL: /compra/AgregarProducto
        [HttpPost]
        public IActionResult AgregarProducto(int IdProducto, int Cantidad)
        {
            var productosString = HttpContext.Session.GetString("CarritoDeCompra");

            var productos = new List<DetalleCompraViewModel>();

            if (!string.IsNullOrEmpty(productosString))
            {
                productos = System.Text.Json.JsonSerializer.Deserialize<List<DetalleCompraViewModel>>(productosString);
            }

            var producto = productos?.FirstOrDefault(x => x.IdProducto == IdProducto);

            if (producto != null)
            {
                producto.Cantidad += Cantidad;
                var importe = Convert.ToDecimal(ViewBag.ImporteTotal);
                ViewBag.ImporteTotal = importe + producto.PrecioUnitario;
            }
            else
            {
                var productoTable = _context.Producto.FirstOrDefault(x => x.Id == IdProducto);

                var productoAux = new DetalleCompraViewModel()
                {
                    Linea = productos.Count + 1,
                    IdProducto = IdProducto,
                    NombreProducto = productoTable.Nombre,
                    PrecioUnitario = productoTable.PrecioVenta,
                    Cantidad = Cantidad
                };
                productos.Add(productoAux);

                var importe = Convert.ToDecimal(ViewBag.ImporteTotal);
                ViewBag.ImporteTotal = importe + productoTable.PrecioVenta * Cantidad;
            }

            productos.RemoveAll(p => p.Cantidad == 0);

            HttpContext.Session.SetString("CarritoDeCompra", System.Text.Json.JsonSerializer.Serialize(productos));

            return RedirectToAction("RegistrarCompra");
        }

        // eliminar producto
        [HttpPost]
        public IActionResult EliminarProducto(int IdProducto)
        {
            var productosString = HttpContext.Session.GetString("CarritoDeCompra");

            var productos = new List<DetalleCompraViewModel>();

            if (!string.IsNullOrEmpty(productosString))
            {
                productos = System.Text.Json.JsonSerializer.Deserialize<List<DetalleCompraViewModel>>(productosString);
            }

            var producto = productos?.FirstOrDefault(x => x.IdProducto == IdProducto);

            if (producto != null)
            {
                productos.Remove(producto);
            }

            HttpContext.Session.SetString("CarritoDeCompra", System.Text.Json.JsonSerializer.Serialize(productos));

            return RedirectToAction("RegistrarVenta");
        }

        // Registrar Compra
        [HttpPost]
        public IActionResult RegistrarCompra(CompraViewModel compraViewModel)
        {
            if (ModelState.IsValid)
            {
                var productosString = HttpContext.Session.GetString("CarritoDeCompra");
                var idUsuario = HttpContext.Session.GetInt32("idUsuario");

                var DetallesCompra = new List<DetalleCompraViewModel>();

                if (!string.IsNullOrEmpty(productosString))
                {
                    DetallesCompra = System.Text.Json.JsonSerializer.Deserialize<List<DetalleCompraViewModel>>(productosString);
                }

                // Crear una instancia de Compra y asignar los valores correspondientes
                var compra = new Compra
                {
                    Fecha = DateTime.Now,
                    IdUsuario = (int)idUsuario,
                    IdProveedor = Convert.ToInt32(compraViewModel.IdProveedor),
                    Importe = DetallesCompra.Sum(x => x.Total())
                };

                // Guardar la venta en la base de datos
                _context.Compra.Add(compra);
                _context.SaveChanges();

                // Recorrer la lista de DetalleVenta y guardar los datos en la base de datos
                
                foreach (var detalleCompra in DetallesCompra)
                {
                    var detalle = new DetalleCompra
                    {
                        IdCompra = compra.Id,
                        IdProducto = detalleCompra.IdProducto,
                        Cantidad = detalleCompra.Cantidad,
                        PrecioUnitario = detalleCompra.PrecioUnitario,
                    };

                    // Actualizar el stock del producto
                    var productoTable = _context.Producto.FirstOrDefault(x => x.Id == detalle.IdProducto);
                    if (productoTable != null)
                    {
                        productoTable.Stock += detalle.Cantidad;
                        _context.Producto.Update(productoTable);
                    }

                    _context.DetalleCompra.Add(detalle);
                }

                _context.SaveChanges();

                DetallesCompra.Clear();

                HttpContext.Session.SetString("CarritoDeCompra", System.Text.Json.JsonSerializer.Serialize(DetallesCompra));

                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, vuelve a la vista con los errores de validación
            var marcas = _context.Marca.ToList();
            var categorias = _context.Categoria.ToList();
            var productos = _context.Producto.ToList();

            var listProductos = new Dictionary<int, string>();
            foreach (var x in productos)
            {
                var nombreCategoria = categorias.FirstOrDefault(y => y.Id == x.IdCategoria)?.Nombre;
                var nombreMarca = marcas.FirstOrDefault(y => y.Id == x.IdMarca)?.Nombre;
                var nombreCategoriaYmarca = $"codBar: {x.CodigoBarras} Cod: {x.Id} -> {nombreCategoria} -> {nombreMarca} -> {x.Nombre}";
                listProductos.Add(x.Id, nombreCategoriaYmarca);
            }

            ViewBag.Productos = listProductos;
            ViewBag.Proveedores = _context.Proveedor.ToList();
            ViewBag.ImporteTotal = 0;

            return View("RegistrarCompra", compraViewModel);
        }
    }
}