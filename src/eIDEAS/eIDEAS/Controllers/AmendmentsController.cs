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
                var loggedInUserID = _userManager.GetUserId(HttpContext.User);

                //Create the amendment
                amendment.UserID = new Guid(loggedInUserID);
                amendment.DateCreated = DateTime.UtcNow;
                _context.Add(amendment);
                await _context.SaveChangesAsync();

                //Give the amendment author 100 participation points
                var loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();
                loggedInUser.ParticipationPoints += 100;
                _context.Update(loggedInUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}
