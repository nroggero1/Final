﻿using Final.Data;
using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Final.Controllers
{
    public class VentaController : Controller
    {
        private readonly FinalWebContext _context;
        public VentaController(FinalWebContext context)
        {
            _context = context;
        }

        // URL: /Venta
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Venta.ToListAsync();
            return View(ventas);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarVenta(int? id)
        {
            if (id == null || _context.Venta == null)
            {
                return NotFound();
            }

            var venta = await _context.Venta.FirstOrDefaultAsync(v => v.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            var detallesVenta = await _context.DetalleVenta.Where(v => v.IdVenta == id).ToListAsync();
            var ventaViewModel = new VentaViewModel()
            {
                 IdCliente = venta.IdCliente,
                  Fecha = venta.Fecha,
                    Importe = venta.Importe
            };

            var detallesVentaViewModel = new List<DetalleVentaViewModel>();
            foreach (var detalleVenta in detallesVenta)
            {
                var detalleVentaViewModel = new DetalleVentaViewModel()
                {
                    IdVenta = detalleVenta.IdVenta,
                    IdProducto = detalleVenta.IdProducto,
                    Cantidad = detalleVenta.Cantidad,
                    PrecioUnitario = detalleVenta.PrecioUnitario
                };

                detallesVentaViewModel.Add(detalleVentaViewModel);
                
            }

            ventaViewModel.DetallesVenta = detallesVentaViewModel;

            return View(ventaViewModel);
        }


        // URL: /Venta/RegistrarVenta
        [HttpGet]
        public Task<IActionResult> RegistrarVenta()
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

            var clientes = _context.Cliente.ToList();
            ViewBag.Clientes = clientes; ;

            ViewBag.ImporteTotal = 0;

            return Task.FromResult<IActionResult>(View());
        }

        // agregar Producto
        [HttpPost]
        public IActionResult AgregarProducto(int IdProducto, int Cantidad)
        {
            var productosString = HttpContext.Session.GetString("CarritoDeVenta");

            var productos = new List<DetalleVentaViewModel>();

            if (!string.IsNullOrEmpty(productosString))
            {
                productos = System.Text.Json.JsonSerializer.Deserialize<List<DetalleVentaViewModel>>(productosString);
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

                var productoAux = new DetalleVentaViewModel()
                {
                    Linea = productos.Count + 1,
                    IdProducto = IdProducto,
                    NombreProducto = productoTable.Nombre,
                    PrecioUnitario = productoTable.PrecioVenta,
                    Cantidad = Cantidad
                };
                productos.Add(productoAux);

                var importe = Convert.ToDecimal(ViewBag.ImporteTotal);
                ViewBag.ImporteTotal = importe + (productoTable.PrecioVenta * Cantidad);
            }

            productos.RemoveAll(p => p.Cantidad == 0);

            HttpContext.Session.SetString("CarritoDeVenta", System.Text.Json.JsonSerializer.Serialize(productos));

            return RedirectToAction("RegistrarVenta");
        }

        // eliminar producto
        [HttpPost]
        public IActionResult EliminarProducto(int IdProducto)
        {
            var productosString = HttpContext.Session.GetString("CarritoDeVenta");

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

            HttpContext.Session.SetString("CarritoDeVenta", System.Text.Json.JsonSerializer.Serialize(productos));

            return RedirectToAction("RegistrarVenta");
        }

        // Registrar Venta
        [HttpPost]
        public IActionResult RegistrarVenta(VentaViewModel ventaViewModel)
        {
            if (ModelState.IsValid)
            {
                var productosString = HttpContext.Session.GetString("CarritoDeVenta");
                var idUsuario = HttpContext.Session.GetInt32("idUsuario");

                var DetallesVenta = new List<DetalleVentaViewModel>();

                if (!string.IsNullOrEmpty(productosString))
                {
                    DetallesVenta = System.Text.Json.JsonSerializer.Deserialize<List<DetalleVentaViewModel>>(productosString);
                }

                // Crear una instancia de Venta y asignar los valores correspondientes
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    IdUsuario = (int)idUsuario,
                    IdCliente = Convert.ToInt32(ventaViewModel.IdCliente),
                    Importe = DetallesVenta.Sum (x=> x.Total())
                };

                // Guardar la venta en la base de datos
                _context.Venta.Add(venta);
                _context.SaveChanges();

                // Recorrer la lista de DetalleVenta y guardar los datos en la base de datos

                foreach (var detalleVenta in DetallesVenta)
                {
                    var detalle = new DetalleVenta
                    {
                        IdVenta = venta.Id,
                        IdProducto = detalleVenta.IdProducto,
                        Cantidad = detalleVenta.Cantidad,
                        PrecioUnitario = detalleVenta.PrecioUnitario
                    };

                    // Actualizar el stock del producto
                    var productoTable = _context.Producto.FirstOrDefault(x => x.Id == detalleVenta.IdProducto);
                    if (productoTable != null)
                    {
                        productoTable.Stock -= detalleVenta.Cantidad;
                        _context.Producto.Update(productoTable);
                    }

                    _context.DetalleVenta.Add(detalle);
                }

                _context.SaveChanges();

                DetallesVenta.Clear();

                HttpContext.Session.SetString("CarritoDeVenta", System.Text.Json.JsonSerializer.Serialize(DetallesVenta));

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
            ViewBag.Clientes = _context.Cliente.ToList();
            ViewBag.ImporteTotal = 0;

            return View("RegistrarVenta", ventaViewModel);
        }
    }
}
