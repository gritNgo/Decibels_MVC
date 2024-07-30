using Decibels.DataAccess.Repository.IRepository;
using Decibels.Models;
using Decibels.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DecibelsWeb.Areas.Customer.Controllers
{
    // populates and displays shopping cart UI
    [Area("customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == userId, includeProperties: "Product")
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetProductPrice(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Quantity);
            }

            return View(ShoppingCartVM);
        }

        private decimal GetProductPrice(ShoppingCart shoppingCart)
        {
            return shoppingCart.Product.Price;
        }
    }
}
