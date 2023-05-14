﻿using Microsoft.AspNetCore.Mvc;
using Final.Models;
using Final.Data;

namespace Final.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly FinalWebContext _context;
        public ProveedorController(FinalWebContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
