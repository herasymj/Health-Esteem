using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
<<<<<<< HEAD
using eIDEAS.Models.Enums;
=======
using Microsoft.AspNetCore.Authorization;
>>>>>>> 33b978a3e0fe8579475699664ce780c6de874ce1

namespace eIDEAS.Controllers
{
    [Authorize]
    public class IdeasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdeasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;           
        }

        // GET: Ideas?filterType={filterType}
        [HttpGet]
        public async Task<IActionResult> Index(String filterType)
        {
            //If a filter type was not specified, default to the "MyIdeas" page.
            filterType = filterType == null ? "MyIdeas" : filterType;

            //Create basic model for the ideas to show. Initially set to have no rows
            IEnumerable<Idea> ideaModel = new List<Idea>();
            //Get the user ID
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);
            
            //Determine what ideas the user wants to see
            if (filterType == "MyIdeas")
            {         
                //Get a model that filters on only the user's ideas
                ideaModel = await _context.Idea.Where(idea => idea.UserID == new Guid(loggedInUserID)).ToListAsync();
            } else if (filterType == "TeamIdeas")
            {
                //Create a model that filters on only the user's team's ideas.
                var loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();
                int unitID = _context.Unit.Where(unit => unit.ID == loggedInUser.UnitID).FirstOrDefault().ID;
                ideaModel = await _context.Idea.Where(idea => idea.UnitID == unitID).ToListAsync();
            }
            //Send the model to the view and return the view
            return View(ideaModel);
        }

        // GET: Ideas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Idea
                .FirstOrDefaultAsync(m => m.ID == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // GET: Ideas/Create
        public IActionResult Create()
        {
            //Obtain the logged in user and their id
            var _loggedInUserID = _userManager.GetUserId(HttpContext.User);
            var loggedInUser = _context.Users.Where(user => user.Id == _loggedInUserID).FirstOrDefault();

            //Obtain the logged in user's unit
            var userUnit = _context.Unit.Where(unit => unit.ID == loggedInUser.UnitID).FirstOrDefault();

            //Store the associated unit's id and name for use in the view
            ViewBag.UnitID = userUnit.ID;
            ViewBag.UnitName = userUnit.Name;

            return View();
        }

        // POST: Ideas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UnitID,Title,Description,SolutionPlan")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                //Get the logged in user's id
                var _loggedInUserID = _userManager.GetUserId(HttpContext.User);

                //Update the idea with information not directly entered by the user
                idea.UserID = new Guid(_loggedInUserID);
                idea.Status = StatusEnum.Plan;
                idea.DateCreated = DateTime.UtcNow;
                idea.DateEdited = DateTime.UtcNow;

                //Adding idea points for user

                //Attempt to add the idea to the database
                _context.Add(idea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(idea);
        }

        // GET: Ideas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Idea.FindAsync(id);
            if (idea == null)
            {
                return NotFound();
            }
            return View(idea);
        }

        // POST: Ideas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,UnitID,Title,Description,SolutionPlan,Status,DateCreated,DateEdited")] Idea idea)
        {
            if (id != idea.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(idea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdeaExists(idea.ID))
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
            return View(idea);
        }

        // GET: Ideas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Idea
                .FirstOrDefaultAsync(m => m.ID == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var idea = await _context.Idea.FindAsync(id);
            _context.Idea.Remove(idea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdeaExists(int id)
        {
            return _context.Idea.Any(e => e.ID == id);
        }
    }
}
