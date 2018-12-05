using Microsoft.AspNetCore.Mvc;
using eIDEAS.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;
using eIDEAS.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace eIDEAS.UnitTests
{
    public class AdminControllerTest
    {
        ApplicationDbContext _context;
        eIDEAS.Controllers.AdminController controller;

        public AdminControllerTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            this.controller = new Controllers.AdminController(_context, userManager.Object);
        }

        [Fact]
        public void HomepageExists()
        {
            ViewResult homePage = controller.Homepage() as ViewResult;
            Assert.NotNull(homePage);
        }

        [Fact]
        public void IndexExists()
        {
            ViewResult indexPage = controller.Index() as ViewResult;
            Assert.NotNull(indexPage);
        }
    }
}
