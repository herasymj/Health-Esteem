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
using Newtonsoft.Json;

namespace eIDEAS.UnitTests
{
    
    public class AmendmentModelTest
    {
        ApplicationDbContext _context;
        
        public AmendmentModelTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
        }

        [Fact]
        public async void TestAmendmentModel()
        {
            
            //Create a new Unit in the database
            Amendment amendment = new Amendment();
            amendment.DateCreated = new DateTime(2018, 10, 16, 11, 59, 59);
            amendment.Comment = "This is a sample comment";
            amendment.IdeaID = 1;
            amendment.UserID = new Guid();

            //Add the model to the database
            _context.Amendment.Add(amendment);

            //Wait for the database to update changes
            await _context.SaveChangesAsync();

            //Confirm that the changes exist in the database.
            IEnumerable<Amendment> amendmentList = _context.Amendment.Where(row => row.ID == amendment.ID).ToList();
            Assert.Equal(amendmentList.ElementAt(0), amendment);

            //Update the model in the database.
            amendment.Comment = "This is an updated comment";

            _context.Amendment.Update(amendment);

            //Wait for changes to be saved.
            await _context.SaveChangesAsync();

            //Query the database again
            amendmentList = _context.Amendment.Where(row => row.ID == amendment.ID).ToList();

            //Ensure that the model has the updated information and updates are working.
            Assert.Equal(amendmentList.ElementAt(0), amendment);

            //Delete the fake data from the database so that the data does not stay in the tables.
            _context.Amendment.Remove(amendment);

            //Wait for the changes to be saved
            await _context.SaveChangesAsync();

            //Ensure that the row doesn't exist
            Assert.Empty(_context.Amendment.Where(row => row.ID == amendment.ID).ToList());
        }
        
    }
        
}
