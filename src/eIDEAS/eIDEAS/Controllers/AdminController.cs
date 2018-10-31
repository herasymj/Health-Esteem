using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace eIDEAS.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Homepage));
        }

        public IActionResult Homepage()
        {
            return View();
        }
    }
}