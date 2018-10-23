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
    [TestCaseOrderer("TestOrderExamples.TestCaseOrdering.PriorityOrderer", "TestOrderExamples")]
    public class IdeaModelTest
    {
        ApplicationDbContext _context;
        
        public IdeaModelTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
        }

        [Fact]
        public async void TestIdeaModel()
        {

            //Create a new Unit in the database
            Idea idea = new Idea();
            idea.DateCreated = new DateTime(2018, 10, 19, 11, 59, 59);
            idea.DateEdited = new DateTime(2018, 10, 19, 11, 59, 59);
            idea.Description = "A test idea description";
            idea.SolutionPlan = "A sample solution plan";
            idea.Status = Models.Enums.StatusEnum.Adopt;
            idea.Title = "A sample idea title for testing";
            idea.UnitID = 1;
            idea.UserID = new Guid();

            //Add the model to the database
            _context.Idea.Add(idea);

            //Wait for the database to update changes
            await _context.SaveChangesAsync();

            //Confirm that the changes exist in the database.
            IEnumerable<Idea> ideaList = _context.Idea.Where(row => row.ID == idea.ID).ToList();
            Assert.Equal(ideaList.ElementAt(0), idea);

            //Update the model in the database.
            idea.Status = Models.Enums.StatusEnum.Abandon;

            _context.Idea.Update(idea);

            //Wait for changes to be saved.
            await _context.SaveChangesAsync();

            //Query the database again
            ideaList = _context.Idea.Where(row => row.ID == idea.ID).ToList();

            //Ensure that the model has the updated information and updates are working.
            Assert.Equal(ideaList.ElementAt(0), idea);

            //Delete the fake data from the database so that the data does not stay in the tables.
            _context.Idea.Remove(idea);

            //Wait for the changes to be saved
            await _context.SaveChangesAsync();

            //Ensure that the row doesn't exist
            Assert.Empty(_context.Idea.Where(row => row.ID == idea.ID).ToList());
        }
        
    }
        
}
