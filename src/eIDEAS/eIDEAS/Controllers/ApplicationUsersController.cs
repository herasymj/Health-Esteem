using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eIDEAS.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //Create a list to store desired user information
            List<ApplicationUserPresentationViewModel> userViewModel = new List<ApplicationUserPresentationViewModel>();

            //Retrieve the users, divisions and units
            var users = await _context.Users.OrderBy(user => user.FirstName).ToListAsync();
            var divisions = await _context.Division.ToListAsync();
            var units = await _context.Unit.ToListAsync();

            //Loop through each retrieved user and add their information
            foreach(var user in users)
            {
                userViewModel.Add(new ApplicationUserPresentationViewModel()
                {
                    ID = new Guid(user.Id),
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    Division = divisions.Where(division => division.ID == user.DivisionID).FirstOrDefault().Name,
                    Unit = units.Where(unit => unit.ID == user.UnitID).FirstOrDefault().Name,
                    IdeaPoints = user.IdeaPoints,
                    ParticpationPoints = user.ParticipationPoints
                });
            }

            return View(userViewModel);
        }
    }
}