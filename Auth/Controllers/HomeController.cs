using Microsoft.AspNetCore.Mvc;

namespace TokenGenerator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}