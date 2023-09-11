using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public double Balance { get; set; }
        public ICollection<Role> Roles { get; set; }
    }

}