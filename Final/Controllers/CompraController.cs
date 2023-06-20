using Final.Data;
using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.InteropServices;

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
        public async Task<IActionResult> Consultar(int? id)
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

            return View(compra);
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

            var usuarios = _context.Usuario.ToList();
            ViewBag.Usuarios = usuarios;

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

            var productos = new List<DetalleVentaViewModel>();

            if (!string.IsNullOrEmpty(productosString))
            {
                productos = System.Text.Json.JsonSerializer.Deserialize<List<DetalleVentaViewModel>>(productosString);
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
                // Crear una instancia de Compra y asignar los valores correspondientes
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    IdUsuario = Convert.ToInt32(compraViewModel.IdUsuario),
                    IdCliente = Convert.ToInt32(compraViewModel.IdProveedor)
                   
                };

                // Guardar la venta en la base de datos
                _context.Venta.Add(venta);
                _context.SaveChanges();

                // Recorrer la lista de DetalleVenta y guardar los datos en la base de datos
                foreach (var detalleVenta in compraViewModel.DetallesCompra)
                {
                    var detalle = new DetalleCompra
                    {
                        IdCompra = compraViewModel.Id,
                        IdProducto = detalleVenta.IdProducto,
                        Cantidad = detalleVenta.Cantidad,
                        PrecioUnitario = detalleVenta.PrecioUnitario,
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
            ViewBag.Usuarios = _context.Usuario.ToList();
            ViewBag.Clientes = _context.Cliente.ToList();
            ViewBag.ImporteTotal = 0;

            return View("RegistrarCompra", compraViewModel);
        }
    }
}