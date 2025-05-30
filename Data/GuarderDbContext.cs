using Microsoft.EntityFrameworkCore;
using Guarder.Models;

namespace Guarder.Data
{
    public class GuarderDbContext : DbContext
    {
        public GuarderDbContext(DbContextOptions<GuarderDbContext> options) : base(options) { }

        public DbSet<Guard> Guards { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<User> Users { get; set; } // User authentication
        public DbSet<Order> Orders { get; set; } // Order CRUD operations
    }
}
