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
    public class StatisticsControllerTest
    {
        ApplicationDbContext _context;
        private eIDEAS.Controllers.StatisticsController controller;

        public StatisticsControllerTest()
        {
            //Setup the database context in the testing envorinment
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            this._context = new ApplicationDbContext(optionsBuilder.Options);
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            this.controller = new Controllers.StatisticsController(_context, userManager.Object);
        }

        [Fact]
        public void LeaderboardPageExists()
        {
            ViewResult indexPage = controller.Leaderboard("Team") as ViewResult;
            Assert.NotNull(indexPage);
        }

        [Fact]
        public void StatisticsPageExists()
        {
            ViewResult indexPage = controller.Index("Global") as ViewResult;
            Assert.NotNull(indexPage);
        }
    }
}
