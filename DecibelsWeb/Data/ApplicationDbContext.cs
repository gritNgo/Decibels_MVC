using DecibelsWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DecibelsWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Configuration for registering ApplicationDbContext to DbContext using options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
    }
}
