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
        public async void EditPageErrorsOnInvalidId()
        {
            int invalidId = -1;
            //Access the edit page
            var actionResult = controller.UpdateStatus(invalidId);
            actionResult.Wait();
            var editPage = actionResult.Result;

            //Ensure a NotFoundResult is returned.
            Assert.True(editPage is NotFoundResult);
        }

        [Fact]
        public async void EditPageReturnsViewIfValid()
        {
            Idea newIdeas = new Idea();
            newIdeas.UserID = new Guid();
            newIdeas.UnitID = 1;
            newIdeas.Title = "rrrr";
            newIdeas.Description = "dasd";
            newIdeas.SolutionPlan = "dsd";
            newIdeas.Status = 0;
            newIdeas.DateCreated = new DateTime(2008, 5, 1, 8, 30, 52);
            newIdeas.DateEdited = new DateTime(2008, 5, 1, 8, 30, 53);

            _context.Idea.Add(newIdeas);
            await _context.SaveChangesAsync();

            //Access the edit page.
            var actionResult = controller.UpdateStatus(newIdeas.ID);
            actionResult.Wait();
            var editPage = actionResult.Result as ViewResult;

            //Ensure that the detail page is returned.
            Assert.NotNull(editPage);

            //Delete the fake data
            _context.Idea.Remove(newIdeas);
        }

        [Fact]
        public void EditPageErrorsOnNullId()
        {
            //Ensure that if a null id is passed a nofound result is returned.
            //Ensure that if an id that does't exist is passed, a Not Found is thrown
            var actionResult = controller.UpdateStatus(null);
            actionResult.Wait();
            var editPage = actionResult.Result;

            //Ensure a NotFoundResult is returned.
            Assert.True(editPage is NotFoundResult);
        }

        [Fact]
        public async void DeletePageErrorsOnInvalidId()
        {
            int invalidId = -1;
            //Access the delete page.
            var actionResult = controller.Delete(invalidId);
            actionResult.Wait();
            var page = actionResult.Result;

            //Ensure that a NotFoundResult is returned.
            Assert.True(page is NotFoundResult);
        }

        [Fact]
        public async void DeletePageReturnsViewIfValid()
        {
            Idea newIdeas = new Idea();
            newIdeas.UserID = new Guid();
            newIdeas.UnitID = 1;
            newIdeas.Title = "rrrr";
            newIdeas.Description = "dasd";
            newIdeas.SolutionPlan = "dsd";
            newIdeas.Status = 0;
            newIdeas.DateCreated = new DateTime(2008, 5, 1, 8 ,30,52);
            newIdeas.DateEdited = new DateTime(2008, 5, 1, 8, 30, 53);
                
            _context.Idea.Add(newIdeas);
            await _context.SaveChangesAsync();

            //Access the delete page.
            var actionResult = controller.Delete(newIdeas.ID);
            actionResult.Wait();
            var page = actionResult.Result as ViewResult;

            //Ensure that a deletepage is returned
            Assert.NotNull(page);

            //Delete the fake data
            _context.Idea.Remove(newIdeas);
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
            //Create a new idea
            Idea newIdeas = new Idea();
            newIdeas.UserID = new Guid();
            newIdeas.UnitID = 1;
            newIdeas.Title = "rrrr";
            newIdeas.Description = "dasd";
            newIdeas.SolutionPlan = "dsd";
            newIdeas.Status = 0;
            newIdeas.DateCreated = new DateTime(2008, 5, 1, 8, 30, 52);
            newIdeas.DateEdited = new DateTime(2008, 5, 1, 8, 30, 53);

            _context.Idea.Add(newIdeas);
            await _context.SaveChangesAsync();

            //Access the delete confirmation page.
            var actionResult = controller.DeleteConfirmed(newIdeas.ID);
            actionResult.Wait();

            //Ensure that the row got deleted.
            Assert.True(_context.Idea.Where(div => div.ID == newIdeas.ID).ToList().Count == 0);
            //Ensure that the user is returned back to the index page.
            //The result from the controller is a redirect.
            var redirect = actionResult.Result;
            Assert.True(redirect is RedirectToActionResult);
        }
    }
}
