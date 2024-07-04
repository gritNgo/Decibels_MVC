using Microsoft.AspNetCore.Mvc;

namespace DecibelsWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
