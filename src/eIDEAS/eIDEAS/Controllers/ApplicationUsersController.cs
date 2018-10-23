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

            //Retrieve the active users, divisions and units
            var users = await _context.Users.Where(user => user.DateDeleted == null).OrderBy(user => user.FirstName).ToListAsync();
            var divisions = await _context.Division.ToListAsync();
            var units = await _context.Unit.ToListAsync();

            //Loop through each retrieved user and add their information
            foreach(var user in users)
            {
                var divisionName = divisions.Where(division => division.ID == user.DivisionID).FirstOrDefault().Name;
                var unitName = units.Where(unit => unit.ID == user.UnitID).FirstOrDefault().Name;
                userViewModel.Add(generateUserViewModel(user, divisionName, unitName));
            }

            return View(userViewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        { 
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var division = await _context.Division.FindAsync(user.DivisionID);
            var unit = await _context.Unit.FindAsync(user.UnitID);
            var userViewModel = generateUserViewModel(user, division.Name, unit.Name);

            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,FirstName,LastName,Email,IdeaPoints,ParticipationPoints")] ApplicationUserPresentationViewModel user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Convert the view model information back to the user model. We do not want to overwrite other user information.
                    var editedUser = await _context.Users.FindAsync(id.ToString());
                    editedUser.Id = user.ID.ToString();
                    editedUser.FirstName = user.FirstName;
                    editedUser.LastName = user.LastName;
                    editedUser.Email = user.Email;
                    editedUser.IdeaPoints = user.IdeaPoints;
                    editedUser.ParticipationPoints = user.ParticipationPoints;

                    //Update the user
                    _context.Update(editedUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        //Check if user exists in the database
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(user => new Guid(user.Id) == id);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            //Create the user presentation view model
            var division = await _context.Division.FindAsync(user.DivisionID);
            var unit = await _context.Unit.FindAsync(user.UnitID);
            var userViewModel = generateUserViewModel(user, division.Name, unit.Name);

            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            //Populate the date deleted field to indicate that this user has been deleted
            var deletedUser = await _context.Users.FindAsync(id.ToString());
            deletedUser.DateDeleted = DateTime.UtcNow;
            _context.Update(deletedUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Create the presentation view for a single user
        private ApplicationUserPresentationViewModel generateUserViewModel(ApplicationUser user, string divisionName, string unitName)
        {
            return new ApplicationUserPresentationViewModel()
            {
                ID = new Guid(user.Id),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Division = divisionName,
                Unit = unitName,
                IdeaPoints = user.IdeaPoints,
                ParticipationPoints = user.ParticipationPoints
            };
        }
    }
}