using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Models;

public class Role : IdentityRole
{
    public string Name { get; set; }
    public bool Status { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<User> Users { get; set; }
}