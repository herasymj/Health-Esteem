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
using eIDEAS.Models.Enums;
using Microsoft.AspNetCore.Authorization;

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
            if (filterType == null)
                filterType = "MyIdeas";

            //Create basic model for the ideas to show. Initially set to have no rows
            List<IdeaPresentationViewModel> ideaViewModel = new List<IdeaPresentationViewModel>();

            //Retrieve the logged in user's information
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);
            ApplicationUser loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();

            //Retrieve the logged in user's unit information
            Unit loggedInUserUnit = _context.Unit.Where(unit => unit.ID == loggedInUser.UnitID).FirstOrDefault();

            //Determine what ideas the user wants to see
            if (filterType == "MyIdeas")
            {         
                //Get a model that filters on the user's ideas
                var ideas = await _context.Idea.Where(idea => idea.UserID.ToString() == loggedInUserID).ToListAsync();

                //Create the idea presentation viewmodel
                foreach(Idea idea in ideas)
                {
                    var ideaPresentation = new IdeaPresentationViewModel
                        {
                            Overview = idea,
                            AuthorFirstName = loggedInUser.FirstName,
                            AuthorLastName = loggedInUser.LastName,
                            UnitName = loggedInUserUnit.Name
                        };

                    ideaViewModel.Add(ideaPresentation);
                }
                ViewBag.PageName = "My Ideas";
            }
            else if (filterType == "TeamIdeas")
            {
                //Get a model that filters on the user's unit's ideas
                var ideas = await _context.Idea.Where(idea => idea.UnitID == loggedInUserUnit.ID).ToListAsync();

                //Create the idea presentation viewmodel
                foreach (Idea idea in ideas)
                {
                    ApplicationUser ideaAuthor = await _context.Users.Where(user => user.Id == idea.UserID.ToString()).FirstOrDefaultAsync();

                    var ideaPresentation = new IdeaPresentationViewModel
                    {
                        Overview = idea,
                        AuthorFirstName = ideaAuthor.FirstName,
                        AuthorLastName = ideaAuthor.LastName,
                        UnitName = loggedInUserUnit.Name
                    };

                    ideaViewModel.Add(ideaPresentation);
                }
                ViewBag.PageName = "Team Ideas";
            }

            //Send the model to the view and return the view
            return View(ideaViewModel);
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

                //Attempt to add the idea to the database
                _context.Add(idea);

                //save changes and return to home
                await _context.SaveChangesAsync();

                //add points
                //int id = idea.ID;
                //var action = new Models.Action();
                //action.UserID = new Guid(_loggedInUserID);
                //action.IdeaID = id;
                //action.Type = ActionTypeEnum.IdeaPoint;
                ////action.Value = 150.ToString();
                //action.Date = DateTime.UtcNow;

                //_context.Add(action);
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,UnitID,Title,Description,SolutionPlan,Status,DateCreated")] Idea idea)
        {
            if (id != idea.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    idea.DateEdited = DateTime.UtcNow;
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
