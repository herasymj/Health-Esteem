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
using Microsoft.AspNetCore.Identity;
using Moq;

namespace eIDEAS.UnitTests
{
    public class HomeControllerTest
    {
        ApplicationDbContext _context;
        private eIDEAS.Controllers.HomeController controller;

        public HomeControllerTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            this.controller = new Controllers.HomeController(_context, userManager.Object);
        }

        [Fact]
        public void LoginPageExists()
        {
            ViewResult indexPage = controller.Index() as ViewResult;
            Assert.NotNull(indexPage);
        }

        [Fact]
        public void ContactPageExists()
        {
            ViewResult contactPage = controller.Contact() as ViewResult;
            Assert.NotNull(contactPage);
        }

        [Fact]
        public void FAQPageExists()
        {
            ViewResult FAQPage = controller.FAQ() as ViewResult;
            Assert.NotNull(FAQPage);
        }

        [Fact]
        public void ErrorPageExists()
        {
            ViewResult aboutPage = controller.Error(404) as ViewResult;
            Assert.NotNull(aboutPage);
        }

        [Fact]
        public void PrivacyPageExists()
        {
            ViewResult aboutPage = controller.Privacy() as ViewResult;
            Assert.NotNull(aboutPage);
        }
    }
}
