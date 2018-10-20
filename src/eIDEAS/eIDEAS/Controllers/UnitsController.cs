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

namespace eIDEAS.Controllers
{
    [Authorize]
    public class UnitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Units
        public async Task<IActionResult> Index()
        {
            List<UnitDivision> uds = new List<UnitDivision>();

            foreach(Unit unit in await _context.Unit.ToListAsync())
            {
                UnitDivision ud = new UnitDivision(_context)
                {
                    unit = unit
                };
                ud.division = ud.GetDivisions(unit.DivisionID).First();
                uds.Add(ud);
            }

            return View(uds);
        }

        // GET: Units/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            return View(new UnitDivision(_context));
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UnitDivision ud)
        {
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
            var unit = await _context.Unit.FindAsync(id);
            _context.Unit.Remove(unit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id)
        {
            return _context.Unit.Any(e => e.ID == id);
        }
    }
}
