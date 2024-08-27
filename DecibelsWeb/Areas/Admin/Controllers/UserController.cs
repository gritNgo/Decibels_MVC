using Microsoft.AspNetCore.Mvc;
using Decibels.DataAccess.Data;
using Decibels.Models;
using Decibels.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Decibels.Models.ViewModels;
using Decibels.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DecibelsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            // Get role of users
            // Role of AspNetUsers table is inside AspNetRoles table and the 2 are joined and mapped by AspNetUserRoles table 
            // access built-on Identity tables with dbContext by removing the 'AspNet' prefix before the table name i.e. AspNetUserRoles > _db.UserRoles
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in objUserList)
            {
                // based on user's roleId from the UserRoles table where both UserId and RoleId of users are mapped, find the role name from Roles table
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                // this role needs to be passed into the data, so add a [NotMapped] property in the ApplicationUser model
                user.Role = roles.FirstOrDefault(u=>u.Id == roleId).Name;  

                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }

            return Json(new { data = objUserList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion

    }
}
