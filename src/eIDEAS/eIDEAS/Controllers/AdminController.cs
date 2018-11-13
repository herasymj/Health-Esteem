using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eIDEAS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public bool IsAdmin()
        {
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);
            ApplicationUser loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();

            return loggedInUser.IsRole(Models.Enums.RoleEnum.Admin);
        }

        public IActionResult Index()
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(Homepage));
        }

        public IActionResult Homepage()
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}