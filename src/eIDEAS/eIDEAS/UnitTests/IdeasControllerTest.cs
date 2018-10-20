using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eIDEAS.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;
using eIDEAS.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authentication;

namespace eIDEAS.UnitTests
{
    public class IdeasControllerTest
    {

        ApplicationDbContext _context;
        eIDEAS.Controllers.IdeasController controller;

        public IdeasControllerTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);

            //Create a sample user
            var users = new List<ApplicationUser>(){
                 new ApplicationUser { Id = "1", FirstName = "Test", LastName = "User" }
                }.AsQueryable();
            //Setup the user manager

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            //Create the usermanager and register the test user.
            userManager.Setup(_ => _.Users).Returns(users);
            userManager.Setup(_ => _.GetUserId(new Mock<ClaimsPrincipal>().Object)).Returns(users.ElementAt(0).Id);

            //Create a mock controller.
            var hc = new Controllers.IdeasController(_context, userManager.Object);
            
            this.controller = hc;
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
            if (_context.Idea.Where(id => id.ID == 1).ToList().Count == 0)
            {
                //If a division with the ID doesn't exist, create one and save the database.
                Idea newIdeas = new Idea();
                newIdeas.ID = 1;
                newIdeas.UserID = new Guid("39b17b12-1e02-422c-849b-fde0ccb205bc");
                newIdeas.UnitID = 1;
                newIdeas.Title = "rrrr";
                newIdeas.Description = "dasd";
                newIdeas.SolutionPlan = "dsd";
                newIdeas.Status = 0;
                newIdeas.DateCreated = new DateTime(2008, 5, 1, 8, 30, 52);
                newIdeas.DateEdited = new DateTime(2008, 5, 1, 8, 30, 53);

                _context.Idea.Add(newIdeas);
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
        public async void EditPageErrorsOnInvalidId()
        {
            //Ensure that if an id that does't exist is passed, a NotFound is thrown
            if (_context.Idea.Where(id => id.ID == 1).ToList().Count > 0)
            {
                _context.Idea.Remove(_context.Idea.Where(id => id.ID == 1).First());
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
            if (_context.Idea.Where(id => id.ID == 1).ToList().Count == 0)
            {
                Idea newIdeas = new Idea();
                newIdeas.ID = 1;
                newIdeas.UserID = new Guid("39b17b12-1e02-422c-849b-fde0ccb205bc");
                newIdeas.UnitID = 1;
                newIdeas.Title = "rrrr";
                newIdeas.Description = "dasd";
                newIdeas.SolutionPlan = "dsd";
                newIdeas.Status = 0;
                newIdeas.DateCreated = new DateTime(2008, 5, 1, 8, 30, 52);
                newIdeas.DateEdited = new DateTime(2008, 5, 1, 8, 30, 53);

                _context.Idea.Add(newIdeas);
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
            if (_context.Idea.Where( id=> id.ID == 1).ToList().Count == 0)
            {
                Idea newIdeas = new Idea();
                newIdeas.ID = 1;
                newIdeas.UserID = new Guid("39b17b12-1e02-422c-849b-fde0ccb205bc");
                newIdeas.UnitID = 1;
                newIdeas.Title = "rrrr";
                newIdeas.Description = "dasd";
                newIdeas.SolutionPlan = "dsd";
                newIdeas.Status = 0;
                newIdeas.DateCreated = new DateTime(2008, 5, 1, 8 ,30,52);
                 newIdeas.DateEdited = new DateTime(2008, 5, 1, 8, 30, 53);
                
                _context.Idea.Add(newIdeas);
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
            if (_context.Idea.Where(div => div.ID == 1).ToList().Count == 0)
            {
                Idea newIdeas = new Idea();
                newIdeas.ID = 1;
                newIdeas.UserID = new Guid("39b17b12-1e02-422c-849b-fde0ccb205bc");
                newIdeas.UnitID = 1;
                newIdeas.Title = "rrrr";
                newIdeas.Description = "dasd";
                newIdeas.SolutionPlan = "dsd";
                newIdeas.Status = 0;
                newIdeas.DateCreated = new DateTime(2008, 5, 1, 8, 30, 52);
                newIdeas.DateEdited = new DateTime(2008, 5, 1, 8, 30, 53);

                _context.Idea.Add(newIdeas);
                await _context.SaveChangesAsync();
            }

            //Access the delete confirmation page.
            var actionResult = controller.DeleteConfirmed(1);
            actionResult.Wait();

            //Ensure that the row got deleted.
            Assert.True(_context.Idea.Where(div => div.ID == 1).ToList().Count == 0);
            //Ensure that the user is returned back to the index page.
            //The result from the controller is a redirect.
            var redirect = actionResult.Result;
            Assert.True(redirect is RedirectToActionResult);
        }
    }
}
