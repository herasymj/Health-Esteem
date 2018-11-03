﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eIDEAS.Data;
using eIDEAS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eIDEAS.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public IActionResult Index(string filterType)
        {
            int totalIdeas;
            int ideasThisMonth;
            int ideasThisYear;
            int totalIdeaPoints;
            int totalParticipationPoints;
            double monthsSinceFirstIdea = DateTime.Today.Subtract(_context.Idea.OrderBy(idea => idea.DateCreated).First().DateCreated).TotalDays / (365 / 12); ;
            double averageIdeasPerMonth;
            double averageParticipationPointsPerMonth;
            double averageIdeaPointsPerMonth;
            Guid loggedInUserID;

            ViewData["FilterType"] = filterType == null ? "Global" : filterType;

            switch (filterType)
            {
                case "Individual":
                    //Retrieve the logged in user's information
                    loggedInUserID = new Guid(_userManager.GetUserId(HttpContext.User));

                    totalIdeas = _context.Idea.Where(idea => idea.UserID == loggedInUserID).Count();
                    ideasThisMonth = _context.Idea.Where(idea => idea.DateCreated.Month == DateTime.Today.Month && idea.DateCreated.Year == DateTime.Today.Year && idea.UserID == loggedInUserID).Count();
                    ideasThisYear = _context.Idea.Where(idea => idea.DateCreated.Year == DateTime.Today.Year && idea.UserID == loggedInUserID).Count();

                    //Sum up the total idea points for all users
                    totalIdeaPoints = _context.Users.Where(user => user.Id == loggedInUserID.ToString()).ToList().Sum(user => user.IdeaPoints);

                    //Sum up the total participation points for all users
                    totalParticipationPoints = _context.Users.Where(user => user.Id == loggedInUserID.ToString()).ToList().Sum(user => user.ParticipationPoints);

                    //Months since first idea.
                    averageIdeasPerMonth = totalIdeas / monthsSinceFirstIdea;

                    averageParticipationPointsPerMonth = totalParticipationPoints / monthsSinceFirstIdea;
                    averageIdeaPointsPerMonth = totalIdeaPoints / monthsSinceFirstIdea;
                    break;
                case "Team":
                    //Retrieve the logged in user's information
                    loggedInUserID = new Guid(_userManager.GetUserId(HttpContext.User));
                    ApplicationUser loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID.ToString()).FirstOrDefault();

                    //Retrieve the logged in user's unit information
                    int loggedInUserUnit = _context.Unit.Where(unit => unit.ID == loggedInUser.UnitID).FirstOrDefault().ID;

                    totalIdeas = _context.Idea.Where(idea => idea.UnitID == loggedInUserUnit).Count();
                    ideasThisMonth = _context.Idea.Where(idea => idea.DateCreated.Month == DateTime.Today.Month && idea.DateCreated.Year == DateTime.Today.Year && idea.UnitID == loggedInUserUnit).Count();
                    ideasThisYear = _context.Idea.Where(idea => idea.DateCreated.Year == DateTime.Today.Year && idea.UnitID == loggedInUserUnit).Count();

                    //Sum up the total idea points for all users
                    totalIdeaPoints = _context.Users.Where(user => user.UnitID == loggedInUserUnit).ToList().Sum(user => user.IdeaPoints);

                    //Sum up the total participation points for all users
                    totalParticipationPoints = _context.Users.Where(user => user.UnitID == loggedInUserUnit).ToList().Sum(user => user.ParticipationPoints);

                    //Months since first idea.
                    averageIdeasPerMonth = totalIdeas / monthsSinceFirstIdea;

                    averageParticipationPointsPerMonth = totalParticipationPoints / monthsSinceFirstIdea;
                    averageIdeaPointsPerMonth = totalIdeaPoints / monthsSinceFirstIdea;
                    break;
                //Add query parameter
                case "Global":
                default:
                    totalIdeas = _context.Idea.Count();
                    ideasThisMonth = _context.Idea.Where(idea => idea.DateCreated.Month == DateTime.Today.Month && idea.DateCreated.Year == DateTime.Today.Year).Count();
                    ideasThisYear = _context.Idea.Where(idea => idea.DateCreated.Year == DateTime.Today.Year).Count();

                    //Sum up the total idea points for all users
                    totalIdeaPoints = _context.Users.ToList().Sum(user => user.IdeaPoints);

                    //Sum up the total participation points for all users
                    totalParticipationPoints = _context.Users.ToList().Sum(user => user.ParticipationPoints);

                    //Months since first idea.
                    averageIdeasPerMonth = totalIdeas / monthsSinceFirstIdea;

                    averageParticipationPointsPerMonth = totalParticipationPoints / monthsSinceFirstIdea;
                    averageIdeaPointsPerMonth = totalIdeaPoints / monthsSinceFirstIdea;
                    break;
        }
            List<StatisticPresentationModel> list = new List<StatisticPresentationModel>();

            

            list.Add(new StatisticPresentationModel("Total Ideas", totalIdeas.ToString()));
            list.Add(new StatisticPresentationModel("Another relevant total stat", "TO DO"));

            list.Add(new StatisticPresentationModel("Ideas This Month", ideasThisMonth.ToString()));
            list.Add(new StatisticPresentationModel("Ideas This Year", ideasThisYear.ToString()));

            list.Add(new StatisticPresentationModel("Total Participation Points", totalParticipationPoints.ToString()));
            list.Add(new StatisticPresentationModel("Total Idea Points", totalIdeaPoints.ToString()));

            list.Add(new StatisticPresentationModel("Average Participation Points Per Month", averageParticipationPointsPerMonth.ToString("0.00")));
            list.Add(new StatisticPresentationModel("Average Idea Points Per Month", averageIdeaPointsPerMonth.ToString("0.00")));

            list.Add(new StatisticPresentationModel("Average Monthly Ideas", averageIdeasPerMonth.ToString("0.00")));
            list.Add(new StatisticPresentationModel("Average Idea Rating", "4.2"));

            return View(list);
        }

        public IActionResult Leaderboard(string filterType)
        {
            List<LeaderboardPresentationViewModel> leaderboardModel = new List<LeaderboardPresentationViewModel>();
            ViewData["LeaderboardType"] = filterType == null ? "Individual" : filterType;

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