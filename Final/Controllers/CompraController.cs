using Final.Data;
using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Final.Controllers
{
    public class CompraController : Controller
    {
        private readonly FinalWebContext _context;

        public CompraController(FinalWebContext context)
        {
            _context = context;
        }

        // GET: /Compra/RegistrarCompra
        [HttpGet]
        public IActionResult RegistrarCompra()
        {
            var marcas = _context.Marca.ToList();
            ViewBag.Marcas = marcas;

            var categorias = _context.Categoria.ToList();
            ViewBag.Categorias = categorias;

            var productos = _context.Producto.ToList();
            ViewBag.Productos = productos;

            var usuarios = _context.Usuario.ToList();
            ViewBag.Usuarios = usuarios;

            var proveedores = _context.Proveedor.ToList();
            ViewBag.Proveedores = proveedores;

            return View();
        }

        [HttpGet]
        public IActionResult CargarProductos(int MarcaId, int CategoriaId)
        {
            var productos = _context.Producto
                .Where(p => p.IdMarca == MarcaId && p.IdCategoria == CategoriaId)
                .ToList();

            var options = new StringBuilder();
            options.Append("<option value=''>Seleccione un producto</option>");

            foreach (var producto in productos)
            {
                options.AppendFormat("<option value='{0}'>{1}</option>", producto.Id, producto.Nombre);
            }

            return Content(options.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCompra(Compra compra)
        {
            if (ModelState.IsValid)
            {
                var marcas = _context.Marca.ToList();
                ViewBag.Marcas = marcas;

                var categorias = _context.Categoria.ToList();
                ViewBag.Categorias = categorias;

                var productos = _context.Producto.ToList();
                ViewBag.Productos = productos;

                var usuarios = _context.Usuario.ToList();
                ViewBag.Usuarios = usuarios;

                var proveedores = _context.Proveedor.ToList();
                ViewBag.Proveedores = proveedores;

                compra.Fecha = System.DateTime.Now;
                _context.Compra.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(compra);
        }
    }
}
