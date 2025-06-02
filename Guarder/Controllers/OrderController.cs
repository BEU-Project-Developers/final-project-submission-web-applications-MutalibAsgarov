using Guarder.Models;
using Microsoft.AspNetCore.Mvc;
using Guarder.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Guarder.Controllers
{
    [Authorize] // Require authentication for all actions
    public class OrderController : Controller
    {
        private readonly GuarderDbContext _context;
        public OrderController(GuarderDbContext context)
        {
            _context = context;
        }        // List all orders (admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            try
            {
                var orders = _context.Orders.ToList();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sifarişləri yükləyərkən xəta baş verdi.";
                return View(new List<Order>());
            }
        }
        // Create order (user)
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sifariş səhifəsini yükləyərkən xəta baş verdi.";
                return View();
            }
        }
        
        [HttpPost]
        public IActionResult Create(Order order)
        {
            try
            {
                // Get current user ID
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
                
                if (user != null)
                {
                    order.UserId = user.Id;
                    order.CreatedAt = DateTime.Now;
                    order.Status = "Pending";
                    
                    // Calculate price based on service type and quantity
                    order.TotalPrice = CalculatePrice(order.ServiceType, order.Quantity, order.StartDate, order.EndDate);
                    
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    return RedirectToAction("UserDashboard", "Account");
                }
                
                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sifarişi əlavə edərkən xəta baş verdi. Zəhmət olmasa yenidən cəhd edin.";
                return View(order);
            }
        }

        [Authorize(Roles = "User")]
        public IActionResult MyOrders()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
                
                if (user != null)
                {
                    var orders = _context.Orders.Where(o => o.UserId == user.Id).OrderByDescending(o => o.CreatedAt).ToList();
                    return View(orders);
                }
                
                return View(new List<Order>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sifarişlərinizi yükləyərkən xəta baş verdi.";
                return View(new List<Order>());
            }
        }

        private decimal CalculatePrice(string serviceType, int quantity, DateTime startDate, DateTime endDate)
        {
            decimal basePrice = serviceType switch
            {
                "Personal" => 50m,      // 50 AZN per guard per day
                "Property" => 40m,      // 40 AZN per guard per day
                "Event" => 60m,         // 60 AZN per guard per day
                "Corporate" => 45m,     // 45 AZN per guard per day
                _ => 50m
            };
            
            var days = Math.Max(1, (endDate - startDate).Days + 1);
            return basePrice * quantity * days;
        }        // Edit order (admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            try
            {
                var order = _context.Orders.Find(id);
                if (order == null) return NotFound();
                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sifariş məlumatlarını yükləyərkən xəta baş verdi.";
                return RedirectToAction("Index");
            }
        }        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sifarişi yeniləyərkən xəta baş verdi.";
                return View(order);
            }
        }
        // Delete order (admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                var order = _context.Orders.Find(id);
                if (order == null) return NotFound();
                return View(order);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sifariş məlumatlarını yükləyərkən xəta baş verdi.";
                return RedirectToAction("Index");
            }
        }        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Order order)
        {
            try
            {
                var dbOrder = _context.Orders.Find(order.Id);
                if (dbOrder == null) return NotFound();
                _context.Orders.Remove(dbOrder);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sifarişi silərkən xəta baş verdi.";
                return RedirectToAction("Index");
            }
        }
    }
}
