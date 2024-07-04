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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "Microphones", DisplayOrder = 1 },
               new Category { Id = 2, Name = "Guitars", DisplayOrder = 2 },
               new Category { Id = 3, Name = "Drums", DisplayOrder = 3 }
               );
        }
    }
}
