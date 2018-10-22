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

        public ActionResult AmendmentsRefreshHandler()
        {
            return View();
        }

        // POST: Amendments/Create
        [HttpPost]
        public async Task<IActionResult> Update(int ideaID, string comment)
        {
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);

            var amendment = new Amendment();

            //Create the amendment
            amendment.IdeaID = ideaID;
            amendment.Comment = comment;
            amendment.UserID = new Guid(loggedInUserID);
            amendment.DateCreated = DateTime.UtcNow;
            _context.Add(amendment);
            await _context.SaveChangesAsync();

            //Give the amendment author 100 participation points
            var loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();
            loggedInUser.ParticipationPoints += 100;
            _context.Update(loggedInUser);
            await _context.SaveChangesAsync();

            //Get and return amendments
            //Retrieve amendments for submitted ideas
            List<AmendmentPresentationViewModel> amendmentViewModel = new List<AmendmentPresentationViewModel>();

            var amendments = _context.Amendment.Where(a => a.IdeaID == ideaID);

            foreach (Amendment amendmentVal in amendments)
            {
                 ApplicationUser amendmentAuthor = _context.Users.Where(user => user.Id == amendment.UserID.ToString()).FirstOrDefault();

                var amendmentPresentation = new AmendmentPresentationViewModel
                {
                    AuthorFirstName = amendmentAuthor.FirstName,
                    AuthorLastName = amendmentAuthor.LastName,
                    Comment = amendmentVal.Comment,
                    PostingDate = amendmentVal.DateCreated
                };

                amendmentViewModel.Add(amendmentPresentation);
            }
            return PartialView("_AmendmentPartial", amendmentViewModel);
        }
    }
}
