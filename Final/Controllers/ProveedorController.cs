using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Final.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly FinalWebContext _context;

        public ProveedorController(FinalWebContext context)
        {
            _context = context;
        }

        // URL: /Proveedor
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var proveedor = await _context.Proveedor.ToListAsync();
            return View(proveedor);
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarProveedor(int? id)
        {
            if (id == null || _context.Proveedor == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor.FirstOrDefaultAsync(p => p.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: /Proveedor/CrearProveedor
        public IActionResult CrearProveedor()
        {
            var provincias = _context.Provincia.ToList();
            ViewBag.Provincias = provincias;

            var localidades = _context.Localidad.ToList();
            ViewBag.Localidades = localidades;

            return View();
        }

        [HttpPost]
        public IActionResult CargarLocalidades(int provinciaId)
        {
            var localidades = _context.Localidad.Where(l => l.IdProvincia == provinciaId).ToList();

            var options = new StringBuilder();
            options.Append("<option value=''>Seleccione una localidad</option>");

            foreach (var localidad in localidades)
            {
                options.AppendFormat("<option value='{0}'>{1}</option>", localidad.Id, localidad.Nombre);
            }

            return Content(options.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> CrearProveedor(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                var provincias = _context.Provincia.ToList();

                ViewBag.Provincias = provincias;
                ViewBag.Localidades = new List<Localidad>();

                proveedor.FechaAlta = System.DateTime.Now;
                proveedor.Activo = true;
                _context.Proveedor.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(proveedor);
        }

        // URL: /Proveedor/EditarProveedor
        [HttpGet]
        public async Task<IActionResult> EditarProveedor(int? id)
        {
            if (id == null || _context.Proveedor == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor.FirstOrDefaultAsync(p => p.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            var provincias = _context.Provincia.ToList();

            ViewBag.Provincias = provincias;
            ViewBag.Localidades = new List<Localidad>();

            return View(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProveedor(int id, Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var proveedorToUpdate = await _context.Proveedor.FirstOrDefaultAsync(c => c.Id == id);
                    proveedorToUpdate.CodigoTributario = proveedor.CodigoTributario;
                    proveedorToUpdate.Direccion = proveedor.Direccion;
                    proveedorToUpdate.IdProvincia = proveedor.IdProvincia;
                    proveedorToUpdate.IdLocalidad = proveedor.IdLocalidad;
                    proveedorToUpdate.Telefono = proveedor.Telefono;
                    proveedorToUpdate.Mail = proveedor.Mail;
                    proveedorToUpdate.Denominacion = proveedor.Denominacion;
                    proveedorToUpdate.Activo = proveedor.Activo;
                    // Mantener el valor original de FechaAlta
                    proveedor.FechaAlta = proveedorToUpdate.FechaAlta;

                    _context.Update(proveedorToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.Id))
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

            var provincias = _context.Provincia.ToList();
            var localidades = _context.Localidad.ToList();

            ViewBag.Provincias = provincias;
            ViewBag.Localidades = localidades;

            return View(proveedor);
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedor.Any(p => p.Id == id);
        }
    }
}
