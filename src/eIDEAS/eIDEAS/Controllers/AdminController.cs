using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eIDEAS.Data;
using eIDEAS.Models;
using eIDEAS.Models.Enums;
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

        // POST: Homepage
        [HttpPost]
        public async Task<IActionResult> HomepageUpdate(string title, string text, MessageEnum type)
        {
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);

            var message = new Message
            {
                AuthorID = new Guid(loggedInUserID),
                Title = title,
                Text = text,
                MessageType = type,
                DateCreated = DateTime.UtcNow
            };
            _context.Add(message);
            await _context.SaveChangesAsync();

            var homeViewModel = new HomePresentationViewModel();

            return Json(homeViewModel);
        }
    }
}