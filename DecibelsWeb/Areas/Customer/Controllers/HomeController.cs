using Decibels.DataAccess.Repository.IRepository;
using Decibels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DecibelsWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Quantity = 1,
                ProductId = productId

            };
            return View(cart);
        }

        [HttpPost]
        [Authorize] // if a user is posting (Adding to cart) they must be authorized regardless of Role
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            // get userID of the logged in user
            var claimsIdentity = (ClaimsIdentity)User.Identity; 
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;

            // check cart doesn't exist for user and productId as both must match same record updates
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
                u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                // shopping cart exists
                cartFromDb.Quantity += shoppingCart.Quantity;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            else
            {
                // add cart record
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
