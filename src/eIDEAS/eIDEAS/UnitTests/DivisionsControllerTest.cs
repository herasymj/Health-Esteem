﻿using Microsoft.AspNetCore.Mvc;
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
            //Check to make sure there is nothing in the database for ID = 1
            if (_context.Division.Where(div => div.ID == 1).ToList().Count > 0)
            {
                //If something exists, remove it.
                _context.Division.Remove(_context.Division.Where(div => div.ID == 1).First());
                await _context.SaveChangesAsync();
            }

            //Access the detail page where id=1 and wait for the page
            var actionResult = controller.Details(1);
            actionResult.Wait();
            var detailPage = actionResult.Result;

            //Ensure that a NotFoundResult is returned.
            Assert.True(detailPage is NotFoundResult);
        }

        [Fact]
        public async void DetailPageReturnsViewIfValid()
        {
            //Check to make sure that a division id exists
            if (_context.Division.Where(div => div.ID == 1).ToList().Count == 0)
            {
                //If a division with the ID doesn't exist, create one and save the database.
                Division newDivision = new Division();
                newDivision.ID = 1;
                newDivision.Name = "Test Name";
                _context.Division.Add(newDivision);
                await _context.SaveChangesAsync();
            }

            //Get the details page for the division
            var actionResult = controller.Details(1);
            actionResult.Wait();
            var detailPage = actionResult.Result as ViewResult;

            //Ensure that a view is returned.
            Assert.NotNull(detailPage);
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
            //Ensure that if an id that does't exist is passed, a NotFound is thrown
            if (_context.Division.Where(div => div.ID == 1).ToList().Count > 0)
            {
                _context.Division.Remove(_context.Division.Where(div => div.ID == 1).First());
                await _context.SaveChangesAsync();
            }

            //Access the edit page
            var actionResult = controller.Edit(1);
            actionResult.Wait();
            var editPage = actionResult.Result;

            //Ensure a NotFoundResult is returned.
            Assert.True(editPage is NotFoundResult);
        }

        [Fact]
        public async void EditPageReturnsViewIfValid()
        {
            //Ensure that if an id that exists is passed, a page is returned
            if (_context.Division.Where(div => div.ID == 1).ToList().Count == 0)
            {
                Division newDivision = new Division();
                newDivision.ID = 1;
                newDivision.Name = "Test Name";
                _context.Division.Add(newDivision);
                await _context.SaveChangesAsync();
            }

            //Access the edit page.
            var actionResult = controller.Edit(1);
            actionResult.Wait();
            var editPage = actionResult.Result as ViewResult;

            //Ensure that the detail page is returned.
            Assert.NotNull(editPage);
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
            //Ensure that if an id that does't exist is passed, a NotFound is thrown
            if (_context.Division.Where(div => div.ID == 1).ToList().Count > 0)
            {
                _context.Division.Remove(_context.Division.Where(div => div.ID == 1).First());
                await _context.SaveChangesAsync();
            }

            //Access the delete page.
            var actionResult = controller.Delete(1);
            actionResult.Wait();
            var page = actionResult.Result;

            //Ensure that a NotFoundResult is returned.
            Assert.True(page is NotFoundResult);
        }

        [Fact]
        public async void DeletePageReturnsViewIfValid()
        {
            //Ensure that if an id that exists is passed, a page is returned
            if (_context.Division.Where(div => div.ID == 1).ToList().Count == 0)
            {
                Division newDivision = new Division();
                newDivision.ID = 1;
                newDivision.Name = "Test Name";
                _context.Division.Add(newDivision);
                await _context.SaveChangesAsync();
            }

            //Access the delete page.
            var actionResult = controller.Delete(1);
            actionResult.Wait();
            var page = actionResult.Result as ViewResult;

            //Ensure that a deletepage is returned
            Assert.NotNull(page);
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
            //Ensure that there is a row to delete.
            if (_context.Division.Where(div => div.ID == 1).ToList().Count == 0)
            {
                Division newDivision = new Division();
                newDivision.ID = 1;
                newDivision.Name = "Test Name";
                _context.Division.Add(newDivision);
                await _context.SaveChangesAsync();
            }

            //Access the delete confirmation page.
            var actionResult = controller.DeleteConfirmed(1);
            actionResult.Wait();

            //Ensure that the row got deleted.
            Assert.True(_context.Division.Where(div => div.ID == 1).ToList().Count == 0);
            //Ensure that the user is returned back to the index page.
            //The result from the controller is a redirect.
            var redirect = actionResult.Result;
            Assert.True(redirect is RedirectToActionResult);
        }
    }
}
