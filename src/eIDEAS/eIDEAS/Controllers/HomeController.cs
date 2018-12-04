using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eIDEAS.Models;
using eIDEAS.Models.Enums;
using eIDEAS.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace eIDEAS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //Get the front page information (What's New and Success Stories)
            var successStory = _context.Message
                .Where(message => message.MessageType == MessageEnum.SuccessStory)
                .OrderByDescending(message => message.DateCreated).FirstOrDefault();
            var whatsNew = _context.Message
                .Where(message => message.MessageType == MessageEnum.WhatsNew)
                .OrderByDescending(message => message.DateCreated).FirstOrDefault();

            var homepageViewModel = new HomePresentationViewModel();
            var success = new Message
            {
                Title = successStory.Title,
                Text = successStory.Text
            };
            var whats = new Message
            {
                Title = whatsNew.Title,
                Text = whatsNew.Text
            };

            homepageViewModel.SuccessStory = success;
            homepageViewModel.WhatsNew = whats;
            return View(homepageViewModel);
        }

        public IActionResult FAQ()
        {

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error(int? id)
        {
            return View(new ErrorViewModel { RequestId = id.ToString() });
        }
    }
}
