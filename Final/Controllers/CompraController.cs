using Final.Data;
using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Final.Controllers
{
    public class CompraController : Controller
    {
        private readonly FinalWebContext _context;

        public CompraController(FinalWebContext context)
        {
            _context = context;
        }

        public IActionResult RegistrarCompra()
        {
            // Obtener la lista de proveedores y productos para mostrar en la vista
            ViewBag.Proveedores = _context.Proveedor.ToList();
            ViewBag.Productos = _context.Producto.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult RegistrarCompra(Compra compra)
        {
            if (ModelState.IsValid)
            {
                // Agregar la compra a la lista de compras
                // Aquí se puede usar una lista en memoria o cualquier otra estructura de datos adecuada
                // Por ejemplo: List<Compra> listaCompras = new List<Compra>();
                // listaCompras.Add(compra);

                // Insertar los registros en la base de datos
                _context.Compra.Add(compra);
                _context.SaveChanges();

                // Recorrer la lista de compras y guardar cada registro en la base de datos
                //foreach (var compra in listaCompras)
                //{
                //    _context.Compras.Add(compra);
                //    _context.SaveChanges();
                //}

                return RedirectToAction(nameof(Index)); // Redirigir a la página de inicio u otra página deseada
            }

            // Si la validación falla, volver a cargar la vista con los errores
            ViewBag.Proveedores = _context.Proveedor.ToList();
            ViewBag.Productos = _context.Producto.ToList();
            return View(compra);
        }
    }
}
