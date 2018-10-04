using eIDEAS.Models;
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

            //Initialize the division
            var divisions = new Division[]
            {
                new Division { Name = "eHealth" },
                new Division { Name = "ENSE 496ab" }
            };
            foreach (Division division in divisions)
            {
                context.Division.Add(division);
            }
            context.SaveChanges();

            //Initialize the units
            var units = new Unit[]
            {
                new Unit{ DivisionID = 1, Name = "Knowledge Management"},
                new Unit{ DivisionID = 1, Name = "Finance"},
                new Unit{ DivisionID = 1, Name = "Continuous Improvement"},
                new Unit{ DivisionID = 1, Name = "Service Desk"},
                new Unit{ DivisionID = 2, Name = "Health Esteem"},
                new Unit{ DivisionID = 2, Name = "Tim's Bits"},
                new Unit{ DivisionID = 2, Name = "!Cool"},
                new Unit{ DivisionID = 2, Name = "MudsnakeFC"}
            };

            foreach (Unit unit in units)
            {
                context.Unit.Add(unit);
            }
            context.SaveChanges();

            //Initialize the users
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
                         DivisionID = 2,
                         UnitID = 5
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
                         DivisionID = 2,
                         UnitID = 5
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
                         DivisionID = 2,
                         UnitID = 5
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
                         DivisionID = 2,
                         UnitID = 5
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
                         DivisionID = 2,
                         UnitID = 5
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
                         DivisionID = 2,
                         UnitID = 5
                     }
                );
            context.SaveChanges();
        }
    }
}
