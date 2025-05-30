using Guarder.Models;
using Microsoft.AspNetCore.Mvc;
using Guarder.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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
            var orders = _context.Orders.ToList();
            return View(orders);
        }
        // Create order (user)
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }        // Edit order (admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();
            return View(order);
        }        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // Delete order (admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();
            return View(order);
        }        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Order order)
        {
            var dbOrder = _context.Orders.Find(order.Id);
            if (dbOrder == null) return NotFound();
            _context.Orders.Remove(dbOrder);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
