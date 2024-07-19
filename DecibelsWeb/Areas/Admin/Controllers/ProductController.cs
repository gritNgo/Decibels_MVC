﻿using Microsoft.AspNetCore.Mvc;
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

        // This annotation identifies an action that supports the HTTP POST method from the Create Product form (When this is absent it's always a GET)
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file) // an object that takes the Product Model properties of the form will be created
        {
            if (ModelState.IsValid) // by checking obj against the Product Model and it's validations
            {
                // Add is an Entity Framework method that tracks the given entity and any changes to be made in the database
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();  // creates the Product on the database

                TempData["success"] = "Product created successfully"; // Displays this message on the next immediate render only

                // Redirects to the Index view which is reloaded once Product is added
                return RedirectToAction("Index", "Product");
            }
            else
            {
                // if ModelState is not valid, populate the dropdown again
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
