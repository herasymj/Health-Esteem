using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace eIDEAS.Controllers
{
    [Authorize]
    public class DivisionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DivisionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public bool IsAdmin()
        {
            var loggedInUserID = _userManager.GetUserId(HttpContext.User);
            ApplicationUser loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID).FirstOrDefault();

            return loggedInUser.IsRole(Models.Enums.RoleEnum.Admin);
        }

        // GET: Divisions
        public async Task<IActionResult> Index()
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(await _context.Division.Where(division => division.DateDeleted == null).ToListAsync());
        }

        // GET: Divisions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Division
                .FirstOrDefaultAsync(m => m.ID == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // GET: Divisions/Create
        public IActionResult Create()
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Divisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Division division)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Add(division);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(division);
        }

        // GET: Divisions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Division.FindAsync(id);
            if (division == null)
            {
                return NotFound();
            }
            return View(division);
        }

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Division division)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != division.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(division);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DivisionExists(division.ID))
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
            return View(division);
        }

        // GET: Divisions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Division
                .FirstOrDefaultAsync(m => m.ID == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var division = await _context.Division.FindAsync(id);
            var units = await _context.Unit.Where(unit => unit.DivisionID == id && unit.DateDeleted == null).ToListAsync();
            if (units.Count != 0)
            {
                //can't delete divisions that still have units
                ViewData["error"] = "Can't delete, there are still units in this division.";
                return View(division);
            }
            else
            {
                ViewData["error"] = null;
            }

            division.DateDeleted = DateTime.UtcNow;
            _context.Division.Update(division);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DivisionExists(int id)
        {
            return _context.Division.Any(e => e.ID == id);
        }
    }
}
