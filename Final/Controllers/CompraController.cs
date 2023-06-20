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

            var compra = await _context.Compra.FirstOrDefaultAsync(v => v.Id == id);
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

            var clientes = _context.Cliente.ToList();
            ViewBag.Clientes = clientes;

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

            // Store the JSON in the session
            HttpContext.Session.SetString("CarritoDeCompra", System.Text.Json.JsonSerializer.Serialize(productos));

            return RedirectToAction("RegistrarCompra");
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

        // Registrar Compra
        [HttpPost]
        public IActionResult RegistrarCompraFinal()
        {
            var productosString = HttpContext.Session.GetString("CarritoDeCompra");

            var compraViewModel = new List<DetalleCompraViewModel>();

            if (!string.IsNullOrEmpty(productosString))
            {
                compraViewModel = System.Text.Json.JsonSerializer.Deserialize<List<DetalleCompraViewModel>>(productosString);
            }

            // Crear una instancia de Venta y asignar los valores correspondientes
            var compra = new Compra
            {
                Fecha = DateTime.Now,
                IdUsuario = 1,
                IdProveedor = 1,
                Importe = Convert.ToDecimal(ViewBag.ImporteTotal)
            };

            // Guardar la venta en la base de datos
            _context.Compra.Add(compra);
            _context.SaveChanges();

            // Recorrer la lista de DetalleCompra y guardar los datos en la base de datos
            foreach (var detalleCompra in compraViewModel)
            {
                var detalle = new DetalleCompra
                {
                    IdCompra = compra.Id,
                    IdProducto = detalleCompra.IdProducto,
                    Cantidad = detalleCompra.Cantidad,
                    PrecioUnitario = detalleCompra.PrecioUnitario
                    // Otros valores de DetalleCompra según tus necesidades
                };
                var productoTable = _context.Producto.FirstOrDefault(x => x.Id == detalleCompra.IdProducto);
                productoTable.Stock = productoTable.Stock + detalleCompra.Cantidad;

                _context.Producto.Update(productoTable);
                _context.DetalleCompra.Add(detalle);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // URL: /Compra/ConsultarCompra
        [HttpGet]
        public async Task<IActionResult> ConsultarCompra(int? id)
        {
            if (id == null || _context.Compra == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FirstOrDefaultAsync(v => v.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }
    }
}
