using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}