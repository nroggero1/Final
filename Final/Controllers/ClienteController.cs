﻿using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Controllers
{
    public class ClienteController : Controller
    {
        private readonly FinalWebContext _context;

        public ClienteController(FinalWebContext context)
        {
            _context = context;
        }


        // URL: /Cliente
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Cliente.ToListAsync();
            return View(clientes);
        }

        [HttpGet]
        public async Task<IActionResult>
            ConsultarCliente(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
            .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: /Cliente/CrearCliente
        public IActionResult CrearCliente()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.FechaAlta = System.DateTime.Now;
                cliente.Activo = true;
                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // URL: /Cliente/EditarCliente
        [HttpGet]
        public async Task<IActionResult> EditarCliente(int? id)
        {
            if (id == null || _context.Cliente == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente.FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var clienteToUpdate = await _context.Cliente.FirstOrDefaultAsync(c => c.Id == id);
                    clienteToUpdate.CodigoTributario = cliente.CodigoTributario;
                    clienteToUpdate.Direccion = cliente.Direccion;
                    clienteToUpdate.LocalidadId = cliente.LocalidadId;
                    clienteToUpdate.Telefono = cliente.Telefono;
                    clienteToUpdate.Mail = cliente.Mail;
                    clienteToUpdate.Denominacion = cliente.Denominacion;
                    clienteToUpdate.Activo = cliente.Activo;
                    // Mantener el valor original de FechaAlta
                    cliente.FechaAlta = clienteToUpdate.FechaAlta;

                    _context.Update(clienteToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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

            return View(cliente);
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(c => c.Id == id);
        }
    }
}
