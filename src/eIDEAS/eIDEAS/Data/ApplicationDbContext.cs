using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using eIDEAS.Models;

namespace eIDEAS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<eIDEAS.Models.Division> Division { get; set; }
        public DbSet<eIDEAS.Models.Unit> Unit { get; set; }
    }
}
