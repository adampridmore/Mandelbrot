using Microsoft.AspNetCore.Mvc;

namespace MandelbrotWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public IActionResult Index()
        {
            return View();
        }
    }
}