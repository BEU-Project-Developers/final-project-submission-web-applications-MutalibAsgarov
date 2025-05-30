using Microsoft.AspNetCore.Mvc;

namespace Guarder.Controllers
{
    public class GuardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
