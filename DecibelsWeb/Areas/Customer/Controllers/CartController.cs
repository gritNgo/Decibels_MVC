using Microsoft.AspNetCore.Mvc;

namespace DecibelsWeb.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        [Area("customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
