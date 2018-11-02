using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eIDEAS.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaderboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(string filterType)
        {
            List<LeaderboardPresentationViewModel> leaderboardModel = new List<LeaderboardPresentationViewModel>();

            switch (filterType)
            {
                case "Team":
                    IEnumerable<ApplicationUser> userList = _context.Users.ToList();

                    foreach (var user in userList)
                    {

                        bool isAdded = false;
                        var unitName = _context.Unit.Where(unit => unit.ID == user.UnitID).FirstOrDefault().Name;

                        foreach (var leaderbord in leaderboardModel)
                        {
                            //Determine if the division exists in the list.
                            
                            if(leaderbord.name == unitName)
                            {
                                leaderbord.ideaPoints += user.IdeaPoints;
                                leaderbord.participationPoints += user.ParticipationPoints;
                                isAdded = true;
                                break;
                            }
                        }

                        if (isAdded == false)
                        {
                            LeaderboardPresentationViewModel leaderboard = new LeaderboardPresentationViewModel();
                            leaderboard.name = unitName;
                            leaderboard.ideaPoints = user.IdeaPoints;
                            leaderboard.participationPoints = user.ParticipationPoints;

                            leaderboardModel.Add(leaderboard);
                        }
                    }
                    break;
                case "Individual":
                default:
                    IEnumerable<ApplicationUser> userLeaderboard = _context.Users.OrderByDescending(user => user.IdeaPoints).ToList();
                    leaderboardModel = userLeaderboard.Select(x => new LeaderboardPresentationViewModel(x)).ToList();
                    break;
            }
            
            return View(leaderboardModel);
        }
    }
}