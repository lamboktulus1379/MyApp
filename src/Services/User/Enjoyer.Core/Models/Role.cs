using Microsoft.AspNetCore.Identity;

namespace Enjoyer.Core.Models;

public class Role : IdentityRole
{
    public string Name { get; set; }
    public bool Status { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}