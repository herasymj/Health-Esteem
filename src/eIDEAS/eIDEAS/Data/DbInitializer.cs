using eIDEAS.Models;
using eIDEAS.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;

namespace eIDEAS.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            //If a division exists, the database has already been initialized
            if (context.Division.Any())
            {
                return;
            }

            //Initialize sample divisions
            context.Division.AddRange(
                new Division { Name = "ENSE 496ab" },
                new Division { Name = "IT Consolidation - Labour Relations" },
                new Division { Name = "Culture, Collaboration, Innovation" },
                new Division { Name = "Risk & Quality" },
                new Division { Name = "Communications" },
                new Division { Name = "Executive" },
                new Division { Name = "Human Resources" },
                new Division { Name = "Information Technology" },
                new Division { Name = "Business Relations and Programs" },
                new Division { Name = "Finance & Administrative" }
            );
            context.SaveChanges();

            //Initialize sample units
            context.Unit.AddRange(
                new Unit { DivisionID = 1, Name = "Health Esteem" },
                new Unit { DivisionID = 1, Name = "Tim's Bits" },
                new Unit { DivisionID = 1, Name = "!Cool" },
                new Unit { DivisionID = 1, Name = "MudsnakeFC" },
                new Unit { DivisionID = 2, Name = "IT Consolidation - Labour Relations" },
                new Unit { DivisionID = 3, Name = "Continuous Improvement" },
                new Unit { DivisionID = 3, Name = "Business Analysis" },
                new Unit { DivisionID = 3, Name = "Portfolio Management" },
                new Unit { DivisionID = 4, Name = "Data Quality" },
                new Unit { DivisionID = 4, Name = "Information Management/Services" },
                new Unit { DivisionID = 4, Name = "Data Warehouse" },
                new Unit { DivisionID = 4, Name = "Records Management" },
                new Unit { DivisionID = 4, Name = "Health Registries" },
                new Unit { DivisionID = 4, Name = "Legal & Policy" },
                new Unit { DivisionID = 4, Name = "IT Security/Risk Management" },
                new Unit { DivisionID = 4, Name = "Privacy" },
                new Unit { DivisionID = 4, Name = "Access Management Services" },
                new Unit { DivisionID = 5, Name = "Communications" },
                new Unit { DivisionID = 5, Name = "Transition Change Management" },
                new Unit { DivisionID = 6, Name = "CEO" },
                new Unit { DivisionID = 6, Name = "VP's and Director of Communications" },
                new Unit { DivisionID = 7, Name = "Human Resources" },
                new Unit { DivisionID = 8, Name = "Client Services" },
                new Unit { DivisionID = 8, Name = "Network & Servers" },
                new Unit { DivisionID = 8, Name = "Service Desk" },
                new Unit { DivisionID = 8, Name = "Applications & Intergrations" },
                new Unit { DivisionID = 8, Name = "Testing" },
                new Unit { DivisionID = 8, Name = "IT Architecture" },
                new Unit { DivisionID = 8, Name = "Database Services" },
                new Unit { DivisionID = 8, Name = "IT Change & Release" },
                new Unit { DivisionID = 9, Name = "Business Relations and Programs" },
                new Unit { DivisionID = 10, Name = "Executive Administration" },
                new Unit { DivisionID = 10, Name = "Finance" },
                new Unit { DivisionID = 10, Name = "Payroll" },
                new Unit { DivisionID = 10, Name = "Contracts & Facilities" }
            );
            context.SaveChanges();

            //Retrieve the sample user division and unit IDs
            var ense496DivisionID = context.Division.Where(division => division.Name == "ENSE 496ab").First().ID;
            var healthEsteemUnitID = context.Unit.Where(unit => unit.Name == "Health Esteem").First().ID;

            //Initialize sample users
            context.Users.AddRange(
                new ApplicationUser
                {
                    UserName = "Oscar@HealthEsteem.ca",
                    NormalizedUserName = "OSCAR@HEALTHESTEEM.CA",
                    Email = "Oscar@HealthEsteem.ca",
                    NormalizedEmail = "OSCAR@HEALTHESTEEM.CA",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEE9w7IV4HKixVxUI2dL9uL7pO/b4bDlYefgEHfXY7ItrQ1Jbvia8xlxbPRDOH++Tsw==", //Abc123!
                    SecurityStamp = "AW74NOXTMIRJLAGYJOL34XFRDUZAKCNJ",
                    ConcurrencyStamp = "b3103776-1b4b-47e0-af12-a35245e52f4c",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    FirstName = "Oscar",
                    LastName = "Lou",
                    DivisionID = ense496DivisionID,
                    UnitID = healthEsteemUnitID,
                    ProfilePic = "~/images/default_profile_pic.png"

                },
                new ApplicationUser
                {
                    UserName = "Tristan@HealthEsteem.ca",
                    NormalizedUserName = "TRISTAN@HEALTHESTEEM.CA",
                    Email = "Tristan@HealthEsteem.ca",
                    NormalizedEmail = "TRISTAN@HEALTHESTEEM.CA",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEE9w7IV4HKixVxUI2dL9uL7pO/b4bDlYefgEHfXY7ItrQ1Jbvia8xlxbPRDOH++Tsw==", //Abc123!
                    SecurityStamp = "AW74NOXTMIRJLAGYJOL34XFRDUZAKCNJ",
                    ConcurrencyStamp = "b3103776-1b4b-47e0-af12-a35245e52f4c",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    FirstName = "Tristan",
                    LastName = "Heisler",
                    DivisionID = ense496DivisionID,
                    UnitID = healthEsteemUnitID,
                    IdeaPoints = 450,
                    ProfilePic = "~/images/default_profile_pic.png",
                    Permissions = 7
                },
                new ApplicationUser
                {
                    UserName = "Jennifer@HealthEsteem.ca",
                    NormalizedUserName = "JENNIFER@HEALTHESTEEM.CA",
                    Email = "Jennifer@HealthEsteem.ca",
                    NormalizedEmail = "JENNIFER@HEALTHESTEEM.CA",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEE9w7IV4HKixVxUI2dL9uL7pO/b4bDlYefgEHfXY7ItrQ1Jbvia8xlxbPRDOH++Tsw==", //Abc123!
                    SecurityStamp = "AW74NOXTMIRJLAGYJOL34XFRDUZAKCNJ",
                    ConcurrencyStamp = "b3103776-1b4b-47e0-af12-a35245e52f4c",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    FirstName = "Jennifer",
                    LastName = "Herasymuik",
                    DivisionID = ense496DivisionID,
                    UnitID = healthEsteemUnitID,
                    Permissions = 1,
                    ProfilePic = "~/images/default_profile_pic.png"
                },
                new ApplicationUser
                {
                    UserName = "Quinn@HealthEsteem.ca",
                    NormalizedUserName = "QUINN@HEALTHESTEEM.CA",
                    Email = "Quinn@HealthEsteem.ca",
                    NormalizedEmail = "QUINN@HEALTHESTEEM.CA",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEE9w7IV4HKixVxUI2dL9uL7pO/b4bDlYefgEHfXY7ItrQ1Jbvia8xlxbPRDOH++Tsw==", //Abc123!
                    SecurityStamp = "AW74NOXTMIRJLAGYJOL34XFRDUZAKCNJ",
                    ConcurrencyStamp = "b3103776-1b4b-47e0-af12-a35245e52f4c",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    FirstName = "Quinn",
                    LastName = "Bast",
                    DivisionID = ense496DivisionID,
                    UnitID = healthEsteemUnitID,
                    ProfilePic = "~/images/default_profile_pic.png",
                    Permissions = 6
                },
                new ApplicationUser
                {
                    UserName = "Shawn@HealthEsteem.ca",
                    NormalizedUserName = "SHAWN@HEALTHESTEEM.CA",
                    Email = "Shawn@HealthEsteem.ca",
                    NormalizedEmail = "SHAWN@HEALTHESTEEM.CA",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEE9w7IV4HKixVxUI2dL9uL7pO/b4bDlYefgEHfXY7ItrQ1Jbvia8xlxbPRDOH++Tsw==", //Abc123!
                    SecurityStamp = "AW74NOXTMIRJLAGYJOL34XFRDUZAKCNJ",
                    ConcurrencyStamp = "b3103776-1b4b-47e0-af12-a35245e52f4c",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    FirstName = "Shawn",
                    LastName = "Clake",
                    DivisionID = ense496DivisionID,
                    UnitID = healthEsteemUnitID,
                    ProfilePic = "~/images/default_profile_pic.png"
                },
                new ApplicationUser
                {
                    UserName = "Wilson@HealthEsteem.ca",
                    NormalizedUserName = "WILSON@HEALTHESTEEM.CA",
                    Email = "Wilson@HealthEsteem.ca",
                    NormalizedEmail = "WILSON@HEALTHESTEEM.CA",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEE9w7IV4HKixVxUI2dL9uL7pO/b4bDlYefgEHfXY7ItrQ1Jbvia8xlxbPRDOH++Tsw==", //Abc123!
                    SecurityStamp = "AW74NOXTMIRJLAGYJOL34XFRDUZAKCNJ",
                    ConcurrencyStamp = "b3103776-1b4b-47e0-af12-a35245e52f4c",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    FirstName = "Wilson",
                    LastName = "Nie",
                    DivisionID = ense496DivisionID,
                    UnitID = healthEsteemUnitID,
                    ProfilePic = "~/images/default_profile_pic.png"
                });
            context.SaveChanges();

            //Initialize sample ideas
            var sampleUser = context.Users.Where(user => user.FirstName == "Tristan").FirstOrDefault();
            var initializationTime = DateTime.UtcNow;

            //Add default stories
            context.Message.AddRange(
                new Message
                {
                    AuthorID = new Guid(sampleUser.Id),
                    Title = "Sample What's New Title",
                    Text = "There is nothing new to announce at this time.",
                    MessageType = MessageEnum.WhatsNew,
                    DateCreated = DateTime.UtcNow
                },
                new Message
                {
                    AuthorID = new Guid(sampleUser.Id),
                    Title = "Sample Success Stories Title",
                    Text = "No success stories are available at this time.",
                    MessageType = MessageEnum.SuccessStory,
                    DateCreated = DateTime.UtcNow
                }
            );
            context.SaveChanges();

            context.Idea.AddRange(
                new Idea
                {
                    UserID = new Guid(sampleUser.Id),
                    UnitID = sampleUser.UnitID,
                    Title = "Sample Idea One",
                    Description = "Sample Description",
                    SolutionPlan = "Sample Solution Plan",
                    Status = StatusEnum.Plan,
                    DateCreated = initializationTime,
                    DateEdited = initializationTime
                }, 
                new Idea
                {
                    UserID = new Guid(sampleUser.Id),
                    UnitID = sampleUser.UnitID,
                    Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    SolutionPlan = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Status = StatusEnum.Plan,
                    DateCreated = initializationTime,
                    DateEdited = initializationTime
                },
                new Idea
                {
                    UserID = new Guid(sampleUser.Id),
                    UnitID = sampleUser.UnitID,
                    Title = "Sample Idea Being Checked",
                    Description = "This idea is being checked!",
                    SolutionPlan = "If not, we have a problem.",
                    Status = StatusEnum.Check,
                    DateCreated = initializationTime,
                    DateEdited = initializationTime
                }
            );
            context.SaveChanges();

            //Get default user's
            var jennID = context.Users.Where(user => user.FirstName == "Jennifer").FirstOrDefault();
            var shawnID = context.Users.Where(user => user.FirstName == "Shawn").FirstOrDefault();
            var wilsonID = context.Users.Where(user => user.FirstName == "Wilson").FirstOrDefault();
            var quinnID = context.Users.Where(user => user.FirstName == "Quinn").FirstOrDefault();

            var idea1 = context.Idea.Where(idea => idea.Title == "Sample Idea Being Checked").FirstOrDefault();
            var idea2 = context.Idea.Where(idea => idea.Title == "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt.").FirstOrDefault();
            var idea3 = context.Idea.Where(idea => idea.Title == "Sample Idea One").FirstOrDefault();

            context.IdeaInteraction.AddRange(
                new IdeaInteraction
                {
                    UserId = new Guid(sampleUser.Id),
                    IdeaID = idea1.ID,
                    IsTracked = true,
                    Rating = 4
                },
                new IdeaInteraction
                {
                    UserId = new Guid(sampleUser.Id),
                    IdeaID = idea2.ID,
                    IsTracked = false,
                    Rating = 1
                },
                new IdeaInteraction
                {
                    UserId = new Guid(sampleUser.Id),
                    IdeaID = idea3.ID
                },
                new IdeaInteraction
                {
                    UserId = new Guid(jennID.Id),
                    IdeaID = idea1.ID,
                    IsTracked = true,
                    Rating = 1
                },
                new IdeaInteraction
                {
                    UserId = new Guid(quinnID.Id),
                    IdeaID = idea1.ID,
                    IsTracked = true,
                    Rating = 2
                },
                new IdeaInteraction
                {
                    UserId = new Guid(shawnID.Id),
                    IdeaID = idea1.ID,
                    IsTracked = true,
                    Rating = 5
                },
                new IdeaInteraction
                {
                    UserId = new Guid(wilsonID.Id),
                    IdeaID = idea1.ID,
                    IsTracked = true,
                    Rating = 4
                },
                new IdeaInteraction
                {
                    UserId = new Guid(jennID.Id),
                    IdeaID = idea2.ID,
                    IsTracked = false,
                    Rating = 5
                },
                new IdeaInteraction
                {
                    UserId = new Guid(quinnID.Id),
                    IdeaID = idea2.ID,
                    IsTracked = false,
                    Rating = 1
                },
                new IdeaInteraction
                {
                    UserId = new Guid(shawnID.Id),
                    IdeaID = idea2.ID,
                    IsTracked = false,
                    Rating = 5
                },
                new IdeaInteraction
                {
                    UserId = new Guid(wilsonID.Id),
                    IdeaID = idea2.ID,
                    IsTracked = false,
                    Rating = 4
                }
            );
            context.SaveChanges();

            
        }
    }
}
