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
        public async Task<IActionResult> Index(string filterType, string filterStyle)
        {
            //Create a list to store ideas
            IQueryable<Idea> ideaQuery;

            //Create basic model for the ideas to show. Initially set to have no rows
            List<IdeaPresentationViewModel> ideaViewModel = new List<IdeaPresentationViewModel>();

            //Retrieve the logged in user's information
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);
            ApplicationUser loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();

            //Retrieve the logged in user's unit information
            Unit loggedInUserUnit = _context.Unit.Where(unit => unit.ID == loggedInUser.UnitID).FirstOrDefault();

            //Create a dictionary to store user information
            Dictionary<Guid, ApplicationUser> userDictionary = new Dictionary<Guid, ApplicationUser>();

            //Create a list to store the filtered idea
            List<Idea> filteredIdeas = new List<Idea>();

            //Determine which ideas the user wants to see
            switch(filterType)
            {
                case "drafts":
                    //Get a model that filters on the user's drafts
                    ideaQuery = _context.Idea.Where(idea => idea.IsDraft && idea.UserID.ToString() == loggedInUserID);

                    //Name the page appropriately
                    ViewBag.PageName = "Drafts";
                    ViewBag.filterType = "Drafts";
                    ViewBag.IsDraft = true;
                    break;
                default:
                    //Get a model that filters on the user's ideas
                    ideaQuery = _context.Idea.Where(idea => !idea.IsDraft && idea.UnitID == loggedInUserUnit.ID);

                    //Name the page appropriately
                    ViewBag.PageName = "Ideas";
                    ViewBag.filterType = "Ideas";
                    ViewBag.IsDraft = false;
                    break;
            }

            //Determine what type of filter the user wants to perform on the view
            switch (filterStyle)
            {
                case "New":
                    filteredIdeas = ideaQuery.Where(idea => DateTime.Today.Subtract(idea.DateCreated).Days <= 7).ToList();
                    break;
                case "Top":
                    //Create a list of the average ratings
                    List<Tuple<double, Idea>> ideaList = new List<Tuple<double, Idea>>();

                    //Loop through all ideas in the current query.
                    foreach (Idea idea in ideaQuery)
                    {
                        var ratingList = _context.IdeaInteraction.Where(i => i.IdeaID == idea.ID).ToList();
                        double avgRating = ratingList.Where(i => i.Rating != 0).Count() == 0 ? -1 : Math.Round(ratingList.Where(i => i.Rating != 0).Select(i => i.Rating).Average(), 1);
                        ideaList.Add(new Tuple<double, Idea>(avgRating, idea));
                    }
                    //Sort the list. Create the comparison between x and y to compare the first element in the tuple, descending order.
                    ideaList.Sort((rating1, rating2) => rating2.Item1.CompareTo(rating1.Item1));
                    filteredIdeas = ideaList.Select(idea => idea.Item2).ToList();
                    break;
                case "Tracked":
                    List<int> trackedIdeaIDs = _context.IdeaInteraction.Where(interaction => interaction.UserId == new Guid(loggedInUserID) && interaction.IsTracked).Select(interaction => interaction.IdeaID).ToList();
                    foreach (Idea idea in ideaQuery)
                    {
                        if(trackedIdeaIDs.Contains(idea.ID))
                        {
                            filteredIdeas.Add(idea);
                        }
                    }
                    break;
                case "Plan":
                    filteredIdeas = ideaQuery.Where(idea => idea.Status == StatusEnum.Plan).ToList();
                    break;
                case "Do":
                    filteredIdeas = ideaQuery.Where(idea => idea.Status == StatusEnum.Do).ToList();
                    break;
                case "Check":
                    filteredIdeas = ideaQuery.Where(idea => idea.Status == StatusEnum.Check).ToList();
                    break;
                case "Adopt":
                    filteredIdeas = ideaQuery.Where(idea => idea.Status == StatusEnum.Adopt).ToList();
                    break;
                case "Adapt":
                    filteredIdeas = ideaQuery.Where(idea => idea.Status == StatusEnum.Adapt).ToList();
                    break;
                case "Abandon":
                    filteredIdeas = ideaQuery.Where(idea => idea.Status == StatusEnum.Abandon).ToList();
                    break;
                case "All":
                default:
                    filteredIdeas = ideaQuery.OrderBy(idea => idea.DateCreated).ToList();
                    break;

            }         

            //Create the idea presentation viewmodel
            foreach (Idea idea in filteredIdeas)
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
                if (filterType != "drafts")
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

                //Get the average rating and if it is tracked
                var ideaInteractions = _context.IdeaInteraction.Where(i => i.IdeaID == idea.ID).ToList();
                var currentUserInteraction = ideaInteractions.Where(interaction => interaction.UserId.ToString() == loggedInUserID).FirstOrDefault();
                //if there is no interaction, create one
                if(currentUserInteraction == null)
                {
                    currentUserInteraction = new IdeaInteraction
                    {
                        IdeaID = idea.ID,
                        UserId = new Guid(loggedInUserID),
                        IsTracked = false
                    };

                    _context.Add(currentUserInteraction);
                    _context.SaveChanges();
                }
                double avgRating = ideaInteractions.Where(i => i.Rating != 0).Count() == 0 ? -1 : Math.Round(ideaInteractions.Where(i => i.Rating != 0).Select(i => i.Rating).Average(), 1);

                //Create the idea presentation
                var ideaPresentation = new IdeaPresentationViewModel
                {
                    Overview = idea,
                    AverageRating = avgRating,
                    UserRating = currentUserInteraction.Rating,
                    IsTracked = currentUserInteraction.IsTracked,
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

            return PartialView();
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

        // POST: Ideas/Status/5
        [HttpPost]
        public async Task<IActionResult> Status(int id, StatusEnum status, string message)
        {
            try
            {
                var idea = await _context.Idea.FindAsync(id);
                idea.Status = status;
                idea.ClosingRemarks = message;
                idea.DateEdited = DateTime.UtcNow;
                _context.Update(idea);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdeaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Json(new { success = true, responseText = "This is fine!" });
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

        //Tracked
        // POST: Idea/Track
        [HttpPost]
        public async Task<bool> TrackStatus(int ideaID, bool tracked)
        {
            //Retrieve the logged in user's information and idea interaction
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);
            var currentUserInteraction = _context.IdeaInteraction.Where(
                interaction => interaction.UserId.ToString() == loggedInUserID
                && interaction.IdeaID == ideaID
                ).FirstOrDefault();

            //if there is no interaction, create one
            if (currentUserInteraction == null)
            {
                currentUserInteraction = new IdeaInteraction
                {
                    IdeaID = ideaID,
                    UserId = new Guid(loggedInUserID),
                    IsTracked = tracked
                };

                _context.IdeaInteraction.Add(currentUserInteraction);
            }
            else //update interaction
            {
                currentUserInteraction.IsTracked = tracked;
                _context.IdeaInteraction.Update(currentUserInteraction);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        //Ratings
        [HttpPost]
        public async Task<IActionResult> Rate(int ideaID, int rating)
        {
            //Get the average rating and if it is tracked
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);
            var currentUserInteraction = _context.IdeaInteraction.Where(
                interaction => interaction.UserId.ToString() == loggedInUserID
                && interaction.IdeaID == ideaID
                ).FirstOrDefault();

            //If there is no interaction, create one and give points
            if (currentUserInteraction == null)
            {
                currentUserInteraction = new IdeaInteraction
                {
                    IdeaID = ideaID,
                    UserId = new Guid(loggedInUserID),
                    IsTracked = false,
                    Rating = rating
                };

                _context.Add(currentUserInteraction);
                await _context.SaveChangesAsync();

                //Give particiption points
                var loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();
                loggedInUser.ParticipationPoints += 50;
                _context.Update(loggedInUser);
                _context.SaveChanges();
            }
            else
            {
                //If the original rating is null, give the rater participation points
                if(!(currentUserInteraction.Rating <= 5 && currentUserInteraction.Rating >= 1))
                {
                    var loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();
                    loggedInUser.ParticipationPoints += 50;
                    _context.Update(loggedInUser);
                    await _context.SaveChangesAsync();
                }
                currentUserInteraction.Rating = rating;
                _context.Update(currentUserInteraction);
                await _context.SaveChangesAsync();
            }

            //Calculate the new average rating
            var ideaInteractions = _context.IdeaInteraction.Where(i => i.IdeaID == ideaID).ToList();
            double avgRating = Math.Round(ideaInteractions.Where(i => i.Rating != 0).Select(i => i.Rating).Average(), 1);

            //Return the new average rating for idea
            return Json(avgRating);
        }
    }
}
