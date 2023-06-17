using Microsoft.AspNetCore.Mvc;
using Final.Models;
using System;
using System.Collections.Generic;
using Final.Data;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Final.ViewModels;
using Microsoft.AspNetCore.Http;

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
            var productos = new List<DetalleVentaViewModel>();
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

            return View(venta);
        }


        // URL: /Venta/RegistrarVenta

        [HttpGet]
        public Task<IActionResult> RegistrarVenta()
        {
            var marcas = _context.Marca.ToList();
            //ViewBag.Marcas = marcas;

            var categorias = _context.Categoria.ToList();
            //ViewBag.Categorias = categorias;

            var productos = _context.Producto.ToList();
            //ViewBag.Productos = productos;


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

            var clientes = _context.Cliente.ToList();
            ViewBag.Clientes = clientes;

            return Task.FromResult<IActionResult>(View());
        }


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
            }

            productos.RemoveAll(p => p.Cantidad == 0);

            // Store the JSON in the session
            HttpContext.Session.SetString("CarritoDeVenta", System.Text.Json.JsonSerializer.Serialize(productos));

            return RedirectToAction("RegistrarVenta");
        }

        [HttpPost]
        public IActionResult EliminarProducto(int IdProducto)
        {
            //TODO: hacer Nico
            return RedirectToAction("RegistrarVenta");
        }


        [HttpPost]
        public IActionResult RegistrarVenta(VentaViewModel ventaViewModel)
        {
            if (ModelState.IsValid)
            {
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    IdUsuario = ventaViewModel.IdUsuario,
                    IdCliente = ventaViewModel.IdCliente
                };

                _context.Venta.Add(venta);
                _context.SaveChanges();

                var productosString = HttpContext.Session.GetString("CarritoDeVenta");

                if (!string.IsNullOrEmpty(productosString))
                {
                    var productos = System.Text.Json.JsonSerializer.Deserialize<List<DetalleVentaViewModel>>(productosString);

                    foreach (var producto in productos)
                    {
                        var detalleVenta = new DetalleVenta
                        {
                            IdVenta = venta.Id,
                            IdProducto = producto.IdProducto,
                            Cantidad = producto.Cantidad,
                            PrecioUnitario = producto.PrecioUnitario
                        };

                        _context.DetalleVenta.Add(detalleVenta);

                        // Reducir el stock del producto en la base de datos
                        var productoDB = _context.Producto.FirstOrDefault(p => p.Id == producto.IdProducto);
                        if (productoDB != null)
                        {
                            productoDB.Stock -= producto.Cantidad;
                        }
                    }

                    _context.SaveChanges();
                }

                // Limpiar el carrito de venta en la sesión
                HttpContext.Session.Remove("CarritoDeVenta");

                return RedirectToAction("Index");
            }

            // Si la validación falla, vuelve a cargar la vista de registro con los datos ingresados anteriormente
            return View(ventaViewModel);
        }
    }
}
