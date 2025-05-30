using Microsoft.AspNetCore.Mvc;

namespace Guarder.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
