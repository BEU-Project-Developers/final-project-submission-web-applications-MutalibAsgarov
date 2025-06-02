using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Guarder.Data;

namespace Guarder.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly GuarderDbContext _context;

        public AdminController(GuarderDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                // Get counts for dashboard
                ViewBag.UserCount = _context.Users.Count();
                ViewBag.OrderCount = _context.Orders.Count();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.UserCount = 0;
                ViewBag.OrderCount = 0;
                TempData["ErrorMessage"] = "Admin paneli məlumatlarını yükləyərkən xəta baş verdi.";
                return View();
            }
        }
    }
}
