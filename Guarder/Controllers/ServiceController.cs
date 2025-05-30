using Microsoft.AspNetCore.Mvc;

namespace Guarder.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
