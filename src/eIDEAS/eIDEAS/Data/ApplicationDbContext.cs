using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using eIDEAS.Models;

namespace eIDEAS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Division> Division { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Idea> Idea { get; set; }
        public DbSet<Action> Action { get; set; }
        public DbSet<Point> Point { get; set; }
    }
}
