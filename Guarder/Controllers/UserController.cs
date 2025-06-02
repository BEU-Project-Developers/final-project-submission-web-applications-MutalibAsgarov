using Guarder.Data;
using Guarder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guarder.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly GuarderDbContext _context;

        public UserController(GuarderDbContext context)
        {
            _context = context;
        }        // GET: User/Manage
        public async Task<IActionResult> Manage()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İstifadəçi siyahısını yükləyərkən xəta baş verdi.";
                return View(new List<User>());
            }
        }        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İstifadəçi məlumatlarını yükləyərkən xəta baş verdi.";
                return RedirectToAction(nameof(Manage));
            }
        }        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İstifadəçi məlumatlarını yükləyərkən xəta baş verdi.";
                return RedirectToAction(nameof(Manage));
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing user to preserve the password
                    var existingUser = await _context.Users.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Update only the allowed fields
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "İstifadəçi məlumatları uğurla yeniləndi.";
                }                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "İstifadəçi məlumatlarını yeniləyərkən xəta baş verdi.";
                    return View(user);
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(user);
        }        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İstifadəçi məlumatlarını yükləyərkən xəta baş verdi.";
                return RedirectToAction(nameof(Manage));
            }
        }        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {                // Check if user has orders
                    var hasOrders = await _context.Orders.AnyAsync(o => o.UserId == id);
                    if (hasOrders)
                    {
                        TempData["ErrorMessage"] = "Bu istifadəçini silmək mümkün deyil. Onun sifarişləri mövcuddur.";
                        return RedirectToAction(nameof(Manage));
                    }

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "İstifadəçi uğurla silindi.";
                }

                return RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İstifadəçini silərkən xəta baş verdi.";
                return RedirectToAction(nameof(Manage));
            }
        }        private bool UserExists(int id)
        {
            try
            {
                return _context.Users.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
