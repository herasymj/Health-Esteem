//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace eIDEAS.UnitTests
//{
//    public class HomeControllerTest
//    {
//        private eIDEAS.Controllers.HomeController controller = new Controllers.HomeController();

//        [Fact]
//        public void LoginPageExists()
//        {
//            ViewResult indexPage = controller.Index() as ViewResult;
//            Assert.NotNull(indexPage);
//        }

//        [Fact]
//        public void ContactPageExists()
//        {
//            ViewResult contactPage = controller.Contact() as ViewResult;
//            Assert.NotNull(contactPage);
//        }

//        [Fact]
//        public void FAQPageExists()
//        {
//            ViewResult FAQPage = controller.FAQ() as ViewResult;
//            Assert.NotNull(FAQPage);
//        }

//        [Fact]
//        public void ErrorPageExists()
//        {
//            ViewResult aboutPage = controller.Error(404) as ViewResult;
//            Assert.NotNull(aboutPage);
//        }

//        [Fact]
//        public void PrivacyPageExists()
//        {
//            ViewResult aboutPage = controller.Privacy() as ViewResult;
//            Assert.NotNull(aboutPage);
//        }
//    }
//}
