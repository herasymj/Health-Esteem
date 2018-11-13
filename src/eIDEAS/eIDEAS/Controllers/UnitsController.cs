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
    public class UnitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public UnitsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        // GET: Units
        public async Task<IActionResult> Index()
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            List<UnitDivision> uds = new List<UnitDivision>();

            foreach(Unit unit in await _context.Unit.Where(unit => unit.DateDeleted == null).ToListAsync())
            {
                UnitDivision ud = new UnitDivision(_context)
                {
                    unit = unit
                };
                ud.division = ud.GetDivisions(unit.DivisionID).Where(division => division.DateDeleted == null).First();
                uds.Add(ud);
            }

            return View(uds);
        }

        // GET: Units/Details/5
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

            var unit = await _context.Unit
                .FirstOrDefaultAsync(m => m.ID == id);
            if (unit == null)
            {
                return NotFound();
            }

            UnitDivision ud = new UnitDivision(_context) { unit = unit };
            ud.division = ud.GetDivisions(unit.DivisionID).First();
            return View(ud);
        }

        // GET: Units/Create
        public IActionResult Create()
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new UnitDivision(_context));
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UnitDivision ud)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Add(ud.unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ud.unit);
        }

        // GET: Units/Edit/5
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

            var unit = await _context.Unit.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            UnitDivision ud = new UnitDivision(_context) { unit = unit };
            ud.division = ud.GetDivisions(unit.DivisionID).First();
            return View(ud);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UnitDivision ud)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != ud.unit.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ud.unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(ud.unit.ID))
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
            return View(ud.unit);
        }

        // GET: Units/Delete/5
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

            var unit = await _context.Unit
                .FirstOrDefaultAsync(m => m.ID == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Is the user actually an admin
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var unit = await _context.Unit.FindAsync(id);
            var users = await _context.Users.Where(user => user.UnitID == id).ToListAsync();
            if (users.Count != 0)
            {
                //can't delete units that still have users
                ViewData["error"] = "Can't delete, there are still users in this unit.";
                return View(unit);
            }
            else
            {
                ViewData["error"] = null;
            }

            unit.DateDeleted = DateTime.UtcNow;
            _context.Unit.Update(unit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id)
        {
            return _context.Unit.Any(e => e.ID == id);
        }
    }
}
