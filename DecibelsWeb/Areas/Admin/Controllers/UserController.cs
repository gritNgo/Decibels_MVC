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

            foreach (var user in objUserList)
            {
                if (user.Company == null)
                {
                    user.Company = new() { Name = ""};
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
