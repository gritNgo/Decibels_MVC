using Microsoft.AspNetCore.Mvc;
using Decibels.DataAccess.Data;
using Decibels.Models;
using Decibels.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DecibelsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        // UnitOfWork internally creates an object/implementation of ProductRepository
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // specify which object/repository being worked on to call methods
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            // Retrieve Categories and convert to SelectListItems by using EF Core PROJECTIONS 
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem 
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(objProductList);
        }

        // When creating a new page from an existing page, first create an action method
        // that will be invoked on the controller. Then the View.
        public IActionResult Create()
        {
            return View();
        }

        // This annotation identifies an action that supports the HTTP POST method from the Create Product form (When this is absent it's always a GET)
        [HttpPost]
        public IActionResult Create(Product obj) // an object that takes the Product Model properties of the form will be created
        {
            if (ModelState.IsValid) // by checking obj against the Product Model and it's validations
            {
                // Add is an Entity Framework method that tracks the given entity and any changes to be made in the database
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();  // creates the Product on the database

                TempData["success"] = "Product created successfully"; // Displays this message on the next immediate render only

                // Redirects to the Index view which is reloaded once Product is added
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound(); // or return an Error View
            }

            Product? productFromDb = _unitOfWork.Product.Get(Product => Product.Id == id);
            /* Other ways to retrieve a record
             * Product? ProductFromDb1 = _db.Categories.FirstOrDefault(Product => Product.Id==id);
             * Product? ProductFromDb2 = _db.Categories.Where(Product => Product.Id == id).FirstOrDefault();
            */

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? ProductFromDb = _unitOfWork.Product.Get(Product => Product.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(Product => Product.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
