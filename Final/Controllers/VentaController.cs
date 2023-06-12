﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult RegistrarVenta(Venta venta)
        {
            venta.Fecha = DateTime.Now;

            using (var connection = new SqlConnection())
            {
                connection.Open();

                var command = new SqlCommand("InsertarVenta", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdUsuario", venta.IdUsuario);
                command.Parameters.AddWithValue("@IdCliente", venta.IdCliente);

                var idVentaParameter = new SqlParameter("@IdVenta", SqlDbType.Int);
                idVentaParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(idVentaParameter);

                command.ExecuteNonQuery();

                var idVenta = (int)idVentaParameter.Value;

                return View(venta);
            }

        }
    }
}
