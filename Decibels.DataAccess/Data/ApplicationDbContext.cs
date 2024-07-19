﻿using Decibels.Models;
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
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "Microphones", DisplayOrder = 1 },
               new Category { Id = 2, Name = "Guitars", DisplayOrder = 2 },
               new Category { Id = 3, Name = "Drums", DisplayOrder = 3 }
               );

            modelBuilder.Entity<Product>().HasData(
                new Product { 
                    Id = 1, 
                    Name = "Yamaha YDM707 B Dynamic Vocal Microphone", 
                    Description = "The Yamaha YDM707 dynamic microphone combines decades of Yamaha's expertise in musical technologies with advanced design to deliver unparalleled sound quality.", 
                    Price = 169, CategoryId = 1, ImageUrl="" },
                new Product
                {
                    Id = 2,
                    Name = "Shure SM7B Dynamic Vocal Studio Microphone",
                    Description = "The Shure SM7B microphone has been the industry standard in radio studios for years. ",
                    Price = 381,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Name = "Sennheiser E 935 dynamic vocal microphone",
                    Description = "When you say Sennheiser, you actually mean the E 835! What the SM58 is to Shure, the E 835 is to Sennheiser, and it remains a battle of the titans for the best place in the stage branch!",
                    Price = 158,
                    CategoryId = 1,
                    ImageUrl = ""
                }, new Product
                {
                    Id = 4,
                    Name = "Cordoba Fusion 5 Edge Burst Electric Acoustic Classical Guitar",
                    Description = "Are you used to an electric guitar or western guitar, but would you still like to have a nylon-string classical guitar as well? Then this Cordoba Fusion 5 Edge Burst is an excellent option.",
                    Price = 509,
                    CategoryId = 2,
                    ImageUrl = ""
                }, new Product
                {
                    Id = 5,
                    Name = "Schecter C-1 FR S SLS Elite Blood Burst Electric Guitar with Sustainiac",
                    Description = "The Schecter C-1 FR S SLS Elite can rightly be called the flagship of the series. The basis of this modern electric guitar is formed by an ash (swamp ash) solid body with a beautiful flame maple top.",
                    Price = 1789,
                    CategoryId = 2,
                    ImageUrl = ""
                }, new Product
                {
                    Id = 6,
                    Name = "Marshall MG50FX 50 Watt 1x12 Transistor Guitar Amplifier Combo",
                    Description = "What is a Marshall without its classic gold-black finish? That's right, nothing! That's why the new MG series is back in this colour scheme with the MG50FX guitar amplifier combo as its showpiece.",
                    Price = 365,
                    CategoryId = 2,
                    ImageUrl = ""
                }, new Product
                {
                    Id = 7,
                    Name = "Pearl RS505BC/C31 Roadshow 5-piece drum kit with 3-piece Sabian cymbal set",
                    Description = "With the Pearl RS505BC Roadshow, the aspiring drummer has everything they need: a drum kit, stands, pedals, cymbals, sticks and a stick bag included! The journey of a successful drumming career starts here.",
                    Price = 799,
                    CategoryId = 3,
                    ImageUrl = ""
                }, new Product
                {
                    Id = 8,
                    Name = "Roland TD-02KV V-Drums electronic drum kit",
                    Description = "The next generation of drummers will go far with the Roland TD-02KV. This compact electronic drum kit has the expressive sounds we know from much more expensive V-Drums but then packed into sixteen ready-to-play sets.",
                    Price = 499,
                    CategoryId = 3,
                    ImageUrl = ""
                }, new Product
                {
                    Id = 9,
                    Name = "Pearl EXX705NBR/C704 Export Black Cherry Glitter 5-piece drum kit",
                    Description = "In the new series of Export drum sets, Pearl delivers this five-piece drum set. Pearl Export drum sets are still the best-selling kits in the world and are well known for their excellent price-quality ratio.",
                    Price = 1089,
                    CategoryId = 3,
                    ImageUrl = ""
                }
                );
        }
    }
}
