using Microsoft.AspNetCore.Mvc;

namespace Guarder.Controllers
{
    public class ContactController : Controller
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
