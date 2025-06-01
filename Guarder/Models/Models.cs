namespace Guarder.Models
{
    public class Guard
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class About
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
