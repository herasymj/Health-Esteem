using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eIDEAS.Controllers
{
    public class IdeasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdeasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;           
        }

        // GET: Ideas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Idea.ToListAsync());
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
                idea.Status = "Plan";
                idea.DateCreated = DateTime.UtcNow;
                idea.DateEdited = DateTime.UtcNow;

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
