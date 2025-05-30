namespace Guarder.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public string Details { get; set; }
        public string Status { get; set; } // Pending, Approved, etc.
    }
}
