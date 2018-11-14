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
            List<Idea> ideaList = new List<Idea>();
            List<double> averageRatings = new List<double>();
            int ratingsGiven;

            double averageIdeaRating;

            ViewData["FilterType"] = filterType == null ? "Global" : filterType;

            switch (filterType)
            {
                case "Individual":
                    //Retrieve the logged in user's information
                    loggedInUserID = new Guid(_userManager.GetUserId(HttpContext.User));

                    totalIdeas = _context.Idea.Where(idea => idea.UserID == loggedInUserID && idea.IsDraft == false).Count();
                    ideasThisMonth = _context.Idea.Where(idea => idea.DateCreated.Month == DateTime.Today.Month && idea.DateCreated.Year == DateTime.Today.Year && idea.UserID == loggedInUserID && idea.IsDraft == false).Count();
                    ideasThisYear = _context.Idea.Where(idea => idea.DateCreated.Year == DateTime.Today.Year && idea.UserID == loggedInUserID && idea.IsDraft == false).Count();

                    //Sum up the total idea points for all users
                    totalIdeaPoints = _context.Users.Where(user => user.Id == loggedInUserID.ToString()).ToList().Sum(user => user.IdeaPoints);

                    //Sum up the total participation points for all users
                    totalParticipationPoints = _context.Users.Where(user => user.Id == loggedInUserID.ToString()).ToList().Sum(user => user.ParticipationPoints);

                    //Months since first idea.
                    averageIdeasPerMonth = totalIdeas / monthsSinceFirstIdea;

                    //Calculate the average idea rating for the individual
                    //Get a list of all ideas the user has created this month.
                    ideaList = _context.Idea.Where(idea => idea.UserID == loggedInUserID && idea.IsDraft == false).ToList();

                    //Determine the average rating for each idea.
                    foreach(Idea idea in ideaList)
                    {
                        //Get all idea interactions for the idea.
                        var ideaInteractions = _context.IdeaInteraction.Where(interactions => interactions.IdeaID == idea.ID && interactions.Rating != 0).ToList();
                        //Get the average rating from the list
                        if (ideaInteractions.Count() == 0)
                        {
                            continue;
                        }
                        var averageRating = ideaInteractions.Average(interaction => interaction.Rating);
                        averageRatings.Add(averageRating);
                    }
                    if(averageRatings.Count() == 0)
                    {
                        averageIdeaRating = 0;
                    }
                    else
                    {
                        averageIdeaRating = averageRatings.Average();
                    }
                    

                    //Determine total number of ratings given
                    ratingsGiven = _context.IdeaInteraction.Where(interaction => interaction.UserId == loggedInUserID && interaction.Rating != 0).Count();

                    averageParticipationPointsPerMonth = totalParticipationPoints / monthsSinceFirstIdea;
                    averageIdeaPointsPerMonth = totalIdeaPoints / monthsSinceFirstIdea;
                    break;
                case "Team":
                    //Retrieve the logged in user's information
                    loggedInUserID = new Guid(_userManager.GetUserId(HttpContext.User));
                    ApplicationUser loggedInUser = _context.Users.Where(user => user.Id == loggedInUserID.ToString()).FirstOrDefault();

                    //Retrieve the logged in user's unit information
                    int loggedInUserUnit = _context.Unit.Where(unit => unit.ID == loggedInUser.UnitID).FirstOrDefault().ID;

                    totalIdeas = _context.Idea.Where(idea => idea.UnitID == loggedInUserUnit && idea.IsDraft == false).Count();
                    ideasThisMonth = _context.Idea.Where(idea => idea.DateCreated.Month == DateTime.Today.Month && idea.DateCreated.Year == DateTime.Today.Year && idea.UnitID == loggedInUserUnit && idea.IsDraft == false).Count();
                    ideasThisYear = _context.Idea.Where(idea => idea.DateCreated.Year == DateTime.Today.Year && idea.UnitID == loggedInUserUnit && idea.IsDraft == false).Count();

                    //Sum up the total idea points for all users
                    totalIdeaPoints = _context.Users.Where(user => user.UnitID == loggedInUserUnit).ToList().Sum(user => user.IdeaPoints);

                    //Sum up the total participation points for all users
                    totalParticipationPoints = _context.Users.Where(user => user.UnitID == loggedInUserUnit).ToList().Sum(user => user.ParticipationPoints);

                    //Months since first idea.
                    averageIdeasPerMonth = totalIdeas / monthsSinceFirstIdea;

                    //Calculate the average idea rating for the individual
                    //Get a list of all ideas the user has created this month.
                    ideaList = _context.Idea.Where(idea => idea.UnitID == loggedInUserUnit && idea.IsDraft == false).ToList();
                    
                    //Determine the average rating for each idea.
                    foreach (Idea idea in ideaList)
                    {
                        //Get all idea interactions for the idea.
                        var ideaInteractions = _context.IdeaInteraction.Where(interactions => interactions.IdeaID == idea.ID && interactions.Rating != 0).ToList();
                        //Get the average rating from the list
                        if (ideaInteractions.Count() == 0)
                        {
                            continue;
                        }
                        var averageRating = ideaInteractions.Average(interaction => interaction.Rating);
                        averageRatings.Add(averageRating);
                    }
                    //Calculate final average rating
                    if (averageRatings.Count == 0)
                    {
                        averageIdeaRating = 0;
                    }
                    else
                    {
                        averageIdeaRating = averageRatings.Average();
                    }

                    //Determine total number of ratings given
                    //Get list of userIDs that are on your team
                    var userIdList = _context.Users.Where(user => user.UnitID == loggedInUserUnit).Select(user => new Guid(user.Id)).ToList();
                    ratingsGiven = _context.IdeaInteraction.Where(interaction => userIdList.Contains(interaction.UserId) && interaction.Rating != 0).Count();

                    averageParticipationPointsPerMonth = totalParticipationPoints / monthsSinceFirstIdea;
                    averageIdeaPointsPerMonth = totalIdeaPoints / monthsSinceFirstIdea;
                    break;
                //Add query parameter
                case "Global":
                default:
                    totalIdeas = _context.Idea.Where(idea => idea.IsDraft == false).Count();
                    ideasThisMonth = _context.Idea.Where(idea => idea.DateCreated.Month == DateTime.Today.Month && idea.DateCreated.Year == DateTime.Today.Year && idea.IsDraft == false).Count();
                    ideasThisYear = _context.Idea.Where(idea => idea.DateCreated.Year == DateTime.Today.Year && idea.IsDraft == false).Count();

                    //Sum up the total idea points for all users
                    totalIdeaPoints = _context.Users.ToList().Sum(user => user.IdeaPoints);

                    //Sum up the total participation points for all users
                    totalParticipationPoints = _context.Users.ToList().Sum(user => user.ParticipationPoints);

                    //Months since first idea.
                    averageIdeasPerMonth = totalIdeas / monthsSinceFirstIdea;

                    //Calculate the average idea rating for the individual
                    //Get a list of all ideas the user has created this month.
                    ideaList = _context.Idea.Where(idea => idea.IsDraft == false).ToList();

                    //Determine the average rating for each idea.
                    foreach (Idea idea in ideaList)
                    {
                        //Get all idea interactions for the idea.
                        var ideaInteractions = _context.IdeaInteraction.Where(interactions => interactions.IdeaID == idea.ID && interactions.Rating != 0).ToList();
                        if(ideaInteractions.Count() == 0)
                        {
                            continue;
                        }
                        //Get the average rating from the list
                        var averageRating = ideaInteractions.Average(interaction => interaction.Rating);
                        averageRatings.Add(averageRating);
                    }
                    //Calculate final average rating
                    averageIdeaRating = averageRatings.Average();

                    //Determine total number of ratings given
                    ratingsGiven = _context.IdeaInteraction.Where(interaction => interaction.Rating != 0).Count();
                    
                    averageParticipationPointsPerMonth = totalParticipationPoints / monthsSinceFirstIdea;
                    averageIdeaPointsPerMonth = totalIdeaPoints / monthsSinceFirstIdea;
                    break;
        }
            List<StatisticPresentationModel> list = new List<StatisticPresentationModel>();
            string prefix = "";
            if(filterType == "Individual")
            {
                prefix = "My";
            } else if(filterType == "Team")
            {
                prefix = "My Team's";
            } else
            {
                prefix = "Total";
            }
            

            list.Add(new StatisticPresentationModel(prefix + " Ideas", totalIdeas.ToString()));
            list.Add(new StatisticPresentationModel(prefix + " Ratings Given", ratingsGiven.ToString()));

            list.Add(new StatisticPresentationModel(prefix + " Ideas This Month", ideasThisMonth.ToString()));
            list.Add(new StatisticPresentationModel(prefix + " Ideas This Year", ideasThisYear.ToString()));

            list.Add(new StatisticPresentationModel(prefix + " Participation Points", totalParticipationPoints.ToString()));
            list.Add(new StatisticPresentationModel(prefix + " Idea Points", totalIdeaPoints.ToString()));

            list.Add(new StatisticPresentationModel(prefix + " Monthly Average Participation Points", averageParticipationPointsPerMonth.ToString("0.00")));
            list.Add(new StatisticPresentationModel(prefix + " Monthly Average Idea Points", averageIdeaPointsPerMonth.ToString("0.00")));

            list.Add(new StatisticPresentationModel(prefix + " Monthly Average Ideas", averageIdeasPerMonth.ToString("0.00")));
            list.Add(new StatisticPresentationModel(prefix + " Average Idea Rating", averageIdeaRating.ToString("0.00")));

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