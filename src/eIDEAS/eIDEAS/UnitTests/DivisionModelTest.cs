using eIDEAS.Data;
using eIDEAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace eIDEAS.UnitTests
{
    public class DivisionModelTest
    {
        ApplicationDbContext _context;

        private List<Division> Divisions{ set; get; }

        [Fact]
        public void DivisionModelTest1()
        {
            var i = 0;
            Divisions = new List<Division>(50);
            Divisions.ForEach(Division =>
            {
                i++;
                Division.ID = i;
                Division.Name = "hahahahah";
                _context.Division.Add(Division);
                IEnumerable<Division> Dig = _context.Division.Where(div => div.ID == i).ToList();
                Division.ID = i;
                Division.Name = "wewewewew";
                Dig = _context.Division.Where(div => div.ID == i).ToList();
                Assert.Equal(Dig.ElementAt(0), Division);
                    //If something exists, remove it.
                _context.Division.Remove(_context.Division.Where(div => div.ID == i).First());

                
            });
            


        }

    

    }
}
