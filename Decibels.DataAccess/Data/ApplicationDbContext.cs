using Decibels.Models;
using Microsoft.EntityFrameworkCore;

namespace Decibels.DataAccess.Data
{
    // Used to establish the connection between the database and Entity Framework 
    public class ApplicationDbContext : DbContext
    {
        // Required configuration to pass connection string as options to DbContext
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
