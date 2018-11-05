using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eIDEAS.Data;

namespace eIDEAS.Models
{
    public class UnitDivision
    {
        public Unit unit { get; set; }
        public Division division { get; set; }

        private readonly ApplicationDbContext _context;
        
        public UnitDivision()
        {

        }

        public UnitDivision(ApplicationDbContext context)
        {
            _context = context;
        }

        // Return all divisions or the division specified
        public List<Division> GetDivisions(int? id)
        {
            if (id == null)
                return _context.Division.Where(division => division.DateDeleted == null).ToList();
            return _context.Division.Where(division => division.ID == id).ToList();
        }
    }
}
