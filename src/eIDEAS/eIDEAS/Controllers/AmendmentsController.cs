using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.AspNetCore.Identity;

namespace eIDEAS.Controllers
{
    public class AmendmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AmendmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Amendments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Create([Bind("IdeaID,Comment")] Amendment amendment)
        {
            if (ModelState.IsValid)
            {
                amendment.UserID = new Guid(_userManager.GetUserId(HttpContext.User));
                amendment.DateCreated = DateTime.UtcNow;

                _context.Add(amendment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
