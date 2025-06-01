using Microsoft.AspNetCore.Mvc;
using Guarder.Models;
using Guarder.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Guarder.Controllers
{
    public class AccountController : Controller
    {
        private readonly GuarderDbContext _context;

        public AccountController(GuarderDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

                if (user.Role == "Admin")
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("UserDashboard", "Account");
            }
            
            ViewBag.Error = "Email və ya şifrə yanlışdır.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string Name, string Email, string Password, string Phone, string Role)
        {
            if (_context.Users.Any(u => u.Email == Email))
            {
                ViewBag.Error = "Bu email artıq qeydiyyatdan keçib.";
                return View();
            }
            
            // Validate role
            if (Role != "User" && Role != "Admin")
            {
                ViewBag.Error = "Düzgün rol seçin.";
                return View();
            }
            
            var user = new User { Name = Name, Email = Email, Password = Password, Phone = Phone, Role = Role };
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Order()
        {
            // Only for authenticated users
            var orders = _context.Orders.ToList();
            return View("/Views/Order/Index.cshtml", orders);
        }

        [Authorize(Roles = "User")]
        public IActionResult UserDashboard()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            
            if (user != null)
            {
                var userOrders = _context.Orders.Where(o => o.UserId == user.Id).ToList();
                ViewBag.UserOrders = userOrders;
                ViewBag.UserName = user.Name;
            }
            
            return View();
        }
    }
}
