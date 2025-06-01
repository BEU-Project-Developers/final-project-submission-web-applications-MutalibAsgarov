using System.ComponentModel.DataAnnotations.Schema;

namespace Guarder.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty; // Personal, Property, Event, Corporate
        public int Quantity { get; set; } // Number of guards
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string AdditionalRequirements { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved, In Progress, Completed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
