namespace Guarder.Models
{
    public class Guard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }

    public class About
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
