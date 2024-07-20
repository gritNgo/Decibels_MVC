using Microsoft.AspNetCore.Mvc;
using Decibels.DataAccess.Data;
using Decibels.Models;
using Decibels.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Decibels.Models.ViewModels;

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
            return View(objProductList);
        }

        public IActionResult Upsert(int? id) // Update + Insert
        {
            // Retrieve Categories and convert to SelectListItems by using EF Core PROJECTIONS 
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            //using ViewBag as the model of this action is Product, not Category
            //ViewBag.categoryList: key       CategoryList: value
            //ViewBag.CategoryList = CategoryList;
            // asp-items in Create View takes an IEnumerable of SelectListItem: ViewBag.categoryList

            ProductVM productVM = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                // create
                return View(productVM);
            }

            else
            {
                // update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid) 
            {
                _unitOfWork.Product.Add(productVM.Product);

                TempData["success"] = "Product created successfully";

                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
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
