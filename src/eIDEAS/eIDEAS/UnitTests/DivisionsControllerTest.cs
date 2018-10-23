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
    public class DivisionsControllerTest
    {

        ApplicationDbContext _context;
        eIDEAS.Controllers.DivisionsController controller;

        public DivisionsControllerTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
            this.controller = new Controllers.DivisionsController(_context);
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
            int i = 999999;
            //Find a row that doesn't exist.
            while(_context.Division.Where(div => div.ID == i).ToList().Count > 0)
            {
                i++;
            }

            //Access the detail page where id=1 and wait for the page
            var actionResult = controller.Details(i);
            var detailPage = actionResult.Result;

            //Ensure that a NotFoundResult is returned.
            Assert.True(detailPage is NotFoundResult);
        }

        [Fact]
        public async void DetailPageReturnsViewIfValid()
        {
            //If a division with the ID doesn't exist, create one and save the database.
            Division newDivision = new Division();
            newDivision.Name = "Test Name";
            _context.Division.Add(newDivision);
            await _context.SaveChangesAsync();

            //Get the details page for the division
            var actionResult = controller.Details(newDivision.ID);
            var detailPage = actionResult.Result as ViewResult;

            //Ensure that a view is returned.
            Assert.NotNull(detailPage);

            //Delete the row
            _context.Division.Remove(newDivision);
        }

        [Fact]
        public void DetailPageErrorsOnNullId()
        {
            //Ensure that if a null id is passed a nofound result is returned.
            //Ensure that if an id that does't exist is passed, a Not Found is thrown
            var actionResult = controller.Details(null);
            var detailPage = actionResult.Result;
            Assert.True(detailPage is NotFoundResult);
        }

        [Fact]
        public void EditPageErrorsOnInvalidId()
        {
            int i = 999999;
            while(_context.Division.Where(div => div.ID == i).ToList().Count > 0)
            {
                i++;
            }

            //Access the edit page
            var actionResult = controller.Edit(i);
            var editPage = actionResult.Result;

            //Ensure a NotFoundResult is returned.
            Assert.True(editPage is NotFoundResult);
        }

        [Fact]
        public async void EditPageReturnsViewIfValid()
        {
            //Create a division to test on
            Division newDivision = new Division();
            newDivision.Name = "Test Name";
            _context.Division.Add(newDivision);
            await _context.SaveChangesAsync();

            //Access the edit page.
            var actionResult = controller.Edit(newDivision.ID);
            var editPage = actionResult.Result as ViewResult;

            //Ensure that the detail page is returned.
            Assert.NotNull(editPage);

            //Delete the division
            _context.Division.Remove(newDivision);
        }

        [Fact]
        public void EditPageErrorsOnNullId()
        {
            //Ensure that if a null id is passed a nofound result is returned.
            //Ensure that if an id that does't exist is passed, a Not Found is thrown
            var actionResult = controller.Edit(null);
            var editPage = actionResult.Result;

            //Ensure a NotFoundResult is returned.
            Assert.True(editPage is NotFoundResult);
        }

        [Fact]
        public async void DeletePageErrorsOnInvalidId()
        {
            int i = 999999;
            while (_context.Division.Where(div => div.ID == i).ToList().Count > 0)
            {
                i++;
            }

            //Access the delete page.
            var actionResult = controller.Delete(i);
            var page = actionResult.Result;

            //Ensure that a NotFoundResult is returned.
            Assert.True(page is NotFoundResult);
        }

        [Fact]
        public async void DeletePageReturnsViewIfValid()
        {
            Division newDivision = new Division();
            newDivision.Name = "Test Name";
            _context.Division.Add(newDivision);
            await _context.SaveChangesAsync();

            //Access the delete page.
            var actionResult = controller.Delete(newDivision.ID);
            actionResult.Wait();
            var page = actionResult.Result as ViewResult;

            //Ensure that a deletepage is returned
            Assert.NotNull(page);

            //Delete the test data
            _context.Division.Remove(newDivision);
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
            Division newDivision = new Division();
            newDivision.Name = "Test Name";
            _context.Division.Add(newDivision);
            await _context.SaveChangesAsync();

            //Access the delete confirmation page.
            var actionResult = controller.DeleteConfirmed(newDivision.ID);
            actionResult.Wait();

            //Ensure that the row got deleted.
            Assert.True(_context.Division.Where(div => div.ID == newDivision.ID).First().DateDeleted != null);
            //Ensure that the user is returned back to the index page.
            //The result from the controller is a redirect.
            var redirect = actionResult.Result;
            Assert.True(redirect is RedirectToActionResult);
        }
    }
}
