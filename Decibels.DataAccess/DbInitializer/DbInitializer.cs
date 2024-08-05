using Azure.Identity;
using Decibels.DataAccess.Data;
using Decibels.Models;
using Decibels.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decibels.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            ApplicationDbContext db, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager; 
            _roleManager = roleManager;
        }

        // creates admin and other Roles, and pushes pending migrations
        public void Initialize()
        {
            // if migrations are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            // create roles if they are not created
            // using 'GetAwaiter().GetResult()' instead of 'await' in order to get the result of the task
            if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Customer).GetAwaiter().GetResult())
            {
                // No need for 'SaveChanges' as CreateAsync takes care of that
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company)).GetAwaiter().GetResult();

            // if there are no Roles, create admin user
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "dustspeck00@gmail.com",
                Email = "dustspeck00@gmail.com",
                Name= "Fiorenso Fernando",
                PhoneNumber = "1231231230",
                Street = "123 my street",
                State = "UT",
                PostalCode = "96024",
                City= "FioLand",
            }, "Admin123!").GetAwaiter().GetResult();

            // once it's created, retrieve by email from db 
            ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u=>u.Email == "dustspeck000@gmail.com");

            _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
        
    }
}
