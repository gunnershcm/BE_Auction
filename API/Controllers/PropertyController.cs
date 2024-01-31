using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PropertyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
