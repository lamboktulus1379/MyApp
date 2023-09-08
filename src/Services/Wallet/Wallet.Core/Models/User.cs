using Microsoft.AspNetCore.Identity;

namespace Wallet.Core.Models;
public class User : IdentityUser
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public double Balance { get; set; }

    public User()
    {
        Id = Guid.NewGuid().ToString();
    }
}