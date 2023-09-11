namespace Auth.Core.Models
{
    public class ApplicationRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
