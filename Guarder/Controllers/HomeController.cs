using Microsoft.AspNetCore.Mvc;

namespace Guarder.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
