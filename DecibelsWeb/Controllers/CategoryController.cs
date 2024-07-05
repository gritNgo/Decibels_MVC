using DecibelsWeb.Data;
using DecibelsWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace DecibelsWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        // When creating a new page, first create an action method
        // that will be invoked on the controller. Then the View
        public IActionResult Create()
        {
            return View();
        }
    }
}
