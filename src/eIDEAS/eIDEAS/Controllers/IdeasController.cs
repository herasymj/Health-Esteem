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
        public async Task<IActionResult> Index(string filterType)
        {
            //Create a list to store ideas
            List <Idea> ideas = new List<Idea>();

            //Create basic model for the ideas to show. Initially set to have no rows
            List<IdeaPresentationViewModel> ideaViewModel = new List<IdeaPresentationViewModel>();

            //Retrieve the logged in user's information
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);
            ApplicationUser loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();

            //Retrieve the logged in user's unit information
            Unit loggedInUserUnit = _context.Unit.Where(unit => unit.ID == loggedInUser.UnitID).FirstOrDefault();

            //Create a dictionary to store user information
            Dictionary<Guid, ApplicationUser> userDictionary = new Dictionary<Guid, ApplicationUser>();

            //Determine which ideas the user wants to see
            switch(filterType)
            {
                case "MyDrafts":
                    //Get a model that filters on the user's drafts
                    ideas = await _context.Idea.Where(idea => idea.IsDraft && idea.UnitID == loggedInUserUnit.ID).ToListAsync();

                    //Name the page appropriately
                    ViewBag.PageName = "My Drafts";
                    ViewBag.IsDraft = true;
                    break;

                case "TeamIdeas":
                    //Get a model that filters on the user's unit's ideas
                    ideas = await _context.Idea.Where(idea => !idea.IsDraft && idea.UnitID == loggedInUserUnit.ID).ToListAsync();

                    //Name the page appropriately
                    ViewBag.PageName = "Team Ideas";
                    ViewBag.IsDraft = false;
                    break;

                case "MyIdeas":
                default:
                    //Get a model that filters on the user's ideas
                    ideas = await _context.Idea.Where(idea => !idea.IsDraft && idea.UserID.ToString() == loggedInUserID).ToListAsync();

                    //Name the page appropriately
                    ViewBag.PageName = "My Ideas";
                    ViewBag.IsDraft = false;
                    break;
            }

            //Create the idea presentation viewmodel
            foreach (Idea idea in ideas)
            {
                //If the user dictionary does not have the idea author, add it
                if (!userDictionary.ContainsKey(idea.UserID))
                {
                    ApplicationUser ideaAuthor = await _context.Users.Where(user => user.Id == idea.UserID.ToString()).FirstOrDefaultAsync();
                    userDictionary.Add(idea.UserID, ideaAuthor);
                }

                //Retrieve amendments for submitted ideas
                List<AmendmentPresentationViewModel> amendmentViewModel = new List<AmendmentPresentationViewModel>();

                //Drafts cannot possibly have amendments yet
                if (filterType != "MyDrafts")
                {
                    var amendments = _context.Amendment.Where(amendment => amendment.IdeaID == idea.ID);

                    foreach (Amendment amendment in amendments)
                    {
                        //If the user dictionary does not have the amendment author, add it
                        if (!userDictionary.ContainsKey(amendment.UserID))
                        {
                            ApplicationUser amendmentAuthor = await _context.Users.Where(user => user.Id == amendment.UserID.ToString()).FirstOrDefaultAsync();
                            userDictionary.Add(amendment.UserID, amendmentAuthor);
                        }

                        var amendmentPresentation = new AmendmentPresentationViewModel
                        {
                            AuthorFirstName = userDictionary[amendment.UserID].FirstName,
                            AuthorLastName = userDictionary[amendment.UserID].LastName,
                            Comment = amendment.Comment,
                            PostingDate = amendment.DateCreated
                        };

                        amendmentViewModel.Add(amendmentPresentation);
                    }
                }

                //Create the idea presentation
                var ideaPresentation = new IdeaPresentationViewModel
                {
                    Overview = idea,
                    AuthorFirstName = userDictionary[idea.UserID].FirstName,
                    AuthorLastName = userDictionary[idea.UserID].LastName,
                    UnitName = loggedInUserUnit.Name,
                    Amendments = amendmentViewModel
                };

                ideaViewModel.Add(ideaPresentation);
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
        public async Task<IActionResult> Create([Bind("ID,UnitID,Title,Description,SolutionPlan")] Idea idea, bool isDraft)
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
                idea.IsDraft = isDraft;

                //Attempt to add the idea to the database
                _context.Add(idea);
                await _context.SaveChangesAsync();

                //Give the idea author 150 idea points on idea submission
                if (!isDraft)
                {
                    var loggedInUser = _context.Users.Where(user => user.Id == _loggedInUserID).FirstOrDefault();
                    loggedInUser.IdeaPoints += 150;
                    _context.Update(loggedInUser);
                    await _context.SaveChangesAsync();
                }

                //Return to the appropriate page
                if (isDraft)
                {
                    return RedirectToAction(nameof(Index), new { filterType = "MyDrafts" });
                }
                return RedirectToAction(nameof(Index));
            }
            return View(idea);
        }

        // GET: Ideas/UpdateStatus/5
        public async Task<IActionResult> UpdateStatus(int? id)
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

        // POST: Ideas/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, [Bind("ID,UserID,UnitID,Title,Description,SolutionPlan,Status,DateCreated,IsDraft,ClosingRemarks")] Idea idea)
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

        // GET: Ideas/UpdateDraft/5
        public async Task<IActionResult> EditDraft(int? id)
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

        // POST: Ideas/UpdateDraft/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDraft(int id, [Bind("ID,UserID,UnitID,Title,Description,SolutionPlan,Status,DateCreated")] Idea idea, bool isDraft)
        {
            if (id != idea.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime currentTime = DateTime.UtcNow;

                    idea.IsDraft = isDraft;
                    idea.DateEdited = currentTime;


                    //Update the creation date and idea points upon idea submission
                    if(!isDraft)
                    {
                        idea.DateCreated = currentTime;

                        var _loggedInUserID = _userManager.GetUserId(HttpContext.User);
                        var loggedInUser = _context.Users.Where(user => user.Id == _loggedInUserID).FirstOrDefault();
                        loggedInUser.IdeaPoints += 150;
                        _context.Update(loggedInUser);
                        await _context.SaveChangesAsync();
                    }

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
                if(isDraft)
                {
                    return RedirectToAction(nameof(Index), new { filterType = "MyDrafts" });
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
