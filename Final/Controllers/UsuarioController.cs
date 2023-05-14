﻿using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;

namespace Final.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly FinalWebContext _context;
        public UsuarioController(FinalWebContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
