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
        public DbSet<Amendment> Amendment { get; set; }
        public DbSet<IdeaInteraction> IdeaInteraction { get; set; }
        public DbSet<Message> Message { get; set; }
    }
}
