﻿using Microsoft.AspNetCore.Mvc;

namespace Guarder.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
