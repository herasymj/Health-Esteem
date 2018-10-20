using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eIDEAS.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;
using eIDEAS.Models;
using eIDEAS.Areas.Identity.Pages.Account;

namespace eIDEAS.UnitTests
{
    public class UnitModelTest
    {

        ApplicationDbContext _context;

        public UnitModelTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
        }
        
        [Fact]
        public async void UnitModelTests()
        {
            //Create a new Unit in the database
            Unit unit = new Unit();
            unit.DivisionID = 1;
            unit.Name = "TestModel";

            //Add the model to the database
            _context.Unit.Add(unit);

            //Wait for the database to update changes
            await _context.SaveChangesAsync();

            //Confirm that the changes exist in the database.
            IEnumerable<Unit> unitList = _context.Unit.Where(row => row.ID == unit.ID).ToList();
            Assert.Equal(unitList.ElementAt(0), unit);

            //Update the model in the database.
            unit.Name = "New Name";

            _context.Unit.Update(unit);

            //Wait for changes to be saved.
            await _context.SaveChangesAsync();

            //Query the database again
            unitList = _context.Unit.Where(row => row.ID == unit.ID).ToList();

            //Ensure that the model has the updated information.
            Assert.Equal(unitList.ElementAt(0), unit);

            //Delete the fake data from the database
            _context.Unit.Remove(unit);

            //Wait for changes to be saved.
            await _context.SaveChangesAsync();

            //Ensure that the data is deleted.
            Assert.Empty(_context.Unit.Where(row => row.ID == unit.ID).ToList());
        }

    }
        
}
