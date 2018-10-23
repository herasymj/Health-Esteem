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
    public class UnitsControllerTest
    {

        ApplicationDbContext _context;
        eIDEAS.Controllers.UnitsController controller;

        public UnitsControllerTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
            this.controller = new Controllers.UnitsController(_context);
        }

        [Fact]
        public void IndexPageExists()
        {
            //Route to the index page and wait for the page to display
            var actionResult = controller.Index();
            actionResult.Wait();

            //Obtain the result from the function and ensure that it is a view
            ViewResult indexPage = actionResult.Result as ViewResult;
            Assert.NotNull(indexPage);
        }

        [Fact]
        public async void DetailPageErrorsOnInvalidId()
        {
            int invalidId = -1;
            //Access the detail page where id=1 and wait for the page
            var actionResult = controller.Details(invalidId);
            actionResult.Wait();
            var detailPage = actionResult.Result;

            //Ensure that a NotFoundResult is returned.
            Assert.True(detailPage is NotFoundResult);
        }

        [Fact]
        public async void DetailPageReturnsViewIfValid()
        {
            //Create a new unit to test on
            Unit newUnit = new Unit();
            newUnit.DivisionID = 1;
            newUnit.Name = "TestUnit";
            _context.Unit.Add(newUnit);
            await _context.SaveChangesAsync();

            //Get the details page for the division
            var actionResult = controller.Details(newUnit.ID);
            actionResult.Wait();
            var detailPage = actionResult.Result as ViewResult;

            //Ensure that a view is returned.
            Assert.NotNull(detailPage);

            //Delete the fake data from the dB
            _context.Unit.Remove(newUnit);
        }

        [Fact]
        public void DetailPageErrorsOnNullId()
        {
            //Ensure that if a null id is passed a nofound result is returned.
            //Ensure that if an id that does't exist is passed, a Not Found is thrown
            var actionResult = controller.Details(null);
            actionResult.Wait();
            var detailPage = actionResult.Result;
            Assert.True(detailPage is NotFoundResult);
        }

        [Fact]
        public async void EditPageErrorsOnInvalidId()
        {
            //Find an id that is not used.
            int invalidId = -1;
            //Access the edit page on the row that doesn't exist
            var actionResult = controller.Edit(invalidId);
            actionResult.Wait();
            var editPage = actionResult.Result;

            //Ensure a NotFoundResult is returned.
            Assert.True(editPage is NotFoundResult);
        }

        [Fact]
        public async void EditPageReturnsViewIfValid()
        {
            Unit newUnit = new Unit();
            newUnit.DivisionID = 1;
            newUnit.Name = "TestUnit";
            _context.Unit.Add(newUnit);
            await _context.SaveChangesAsync();

            //Access the edit page.
            var actionResult = controller.Edit(newUnit.ID);
            actionResult.Wait();
            var editPage = actionResult.Result as ViewResult;

            //Ensure that the detail page is returned.
            Assert.NotNull(editPage);

            //Remove the new unit from the database.
            _context.Unit.Remove(newUnit);
        }

        [Fact]
        public void EditPageErrorsOnNullId()
        {
            //Ensure that if a null id is passed a nofound result is returned.
            //Ensure that if an id that does't exist is passed, a Not Found is thrown
            var actionResult = controller.Edit(null);
            actionResult.Wait();
            var editPage = actionResult.Result;

            //Ensure a NotFoundResult is returned.
            Assert.True(editPage is NotFoundResult);
        }

        [Fact]
        public async void DeletePageErrorsOnInvalidId()
        {
            int invalidId = -1;
            //Access the delete page for an entry that doesn't exist.
            var actionResult = controller.Delete(invalidId);
            actionResult.Wait();
            var page = actionResult.Result;

            //Ensure that a NotFoundResult is returned.
            Assert.True(page is NotFoundResult);
        }

        [Fact]
        public async void DeletePageReturnsViewIfValid()
        {
            Unit newUnit = new Unit();
            newUnit.DivisionID = 1;
            newUnit.Name = "TestUnit";
            _context.Unit.Add(newUnit);
            await _context.SaveChangesAsync();
            

            //Access the delete page.
            var actionResult = controller.Delete(newUnit.ID);
            actionResult.Wait();
            var page = actionResult.Result as ViewResult;

            //Ensure that a deletepage is returned
            Assert.NotNull(page);

            //Remove the new row from the database
            _context.Unit.Remove(newUnit);
        }

        [Fact]
        public void DeletePageErrorsOnNullId()
        {
            //Ensure that if a null id is passed a nofound result is returned.
            //Ensure that if an id that does't exist is passed, a Not Found is thrown
            var actionResult = controller.Delete(null);
            actionResult.Wait();
            var detailPage = actionResult.Result;
            Assert.True(detailPage is NotFoundResult);
        }

        [Fact]
        public async void DeleteConfirmedDeletesData()
        {
            Unit newUnit = new Unit();
            newUnit.DivisionID = 1;
            newUnit.Name = "TestUnit";
            _context.Unit.Add(newUnit);
            await _context.SaveChangesAsync();
            

            //Access the delete confirmation page.
            var actionResult = controller.DeleteConfirmed(newUnit.ID);
            actionResult.Wait();

            //Ensure that the row got deleted.
            Assert.True(_context.Unit.Where(unit => unit.ID == newUnit.ID).First().DateDeleted != null);
            //Ensure that the user is returned back to the index page.
            //The result from the controller is a redirect.
            var redirect = actionResult.Result;
            Assert.True(redirect is RedirectToActionResult);

            //Delete the data from the database
            _context.Unit.Remove(newUnit);
        }
    }
}
