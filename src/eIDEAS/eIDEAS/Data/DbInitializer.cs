using eIDEAS.Models;
using System.Linq;

namespace eIDEAS.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            //If a division exists, the database has already been initialized
            if(context.Division.Any())
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

            foreach(Unit unit in units)
            {
                context.Unit.Add(unit);
            }
            context.SaveChanges();
        }
    }
}
