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

        // When creating a new from an existing page, first create an action method
        // that will be invoked on the controller. Then the View.
        public IActionResult Create()
        {
            return View();
        }

        // This annotation identifies an action that supports the HTTP POST method from the Create Category form
        [HttpPost]
        public IActionResult Create(Category obj) // an object that takes the Category Model properties of the form will be created
        {
            if (ModelState.IsValid) // by checking obj against the Category Model and it's validations
            {
                _db.Categories.Add(obj); // Add is an Entity Framework method that tracks the given entity and any changes to be made in the database
                _db.SaveChanges();  // creates the Category on the database
                // Redirects to the Index view which is reloaded once Category is added
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
    }
}
