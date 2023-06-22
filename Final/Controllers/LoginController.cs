using Final.Data;
using Final.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    public class LoginController : Controller
    {
        private readonly FinalWebContext _context;
        public LoginController(FinalWebContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = (_context.Usuario.FirstOrDefault(x => x.NombreUsuario == loginViewModel.Usuario));
                //Valida el usuario con la base de datos
                if (usuario != null && usuario.Clave == loginViewModel.Clave && usuario.Activo == true)
                {
                    HttpContext.Session.SetString("nombreUsuario", usuario.NombreUsuario);
                    HttpContext.Session.SetString("administrador", usuario.Administrador.ToString());
                    HttpContext.Session.SetString("nombre", usuario.Nombre);
                    HttpContext.Session.SetInt32("idUsuario", usuario.Id);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña erroneos");
                }
            }
            else
            {
                ModelState.AddModelError("", "Usuario o contraseña erroneos");
            }

            return View("Index");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Redirect("Index");
        }
    }
}
