using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace eIDEAS.UnitTests
{
    public class HomeControllerTest
    {
        [Fact]
        public void LoginPageExists()
        {
            eIDEAS.Controllers.HomeController controller = new Controllers.HomeController();
            ViewResult indexPage = controller.Index() as ViewResult;
            Assert.NotNull(indexPage);
        }

        [Fact]
        public void ContactPageExists()
        {
            eIDEAS.Controllers.HomeController controller = new Controllers.HomeController();
            ViewResult contactPage = controller.Contact() as ViewResult;
            Assert.NotNull(contactPage);
        }

        [Fact]
        public void AboutPageExists()
        {
            eIDEAS.Controllers.HomeController controller = new Controllers.HomeController();
            ViewResult aboutPage = controller.About() as ViewResult;
            Assert.NotNull(aboutPage);
        }
    }
}
