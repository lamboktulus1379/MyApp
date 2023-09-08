using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enjoyer.Core.Models
{
    public class Login
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Email not correctly formatted")]
        [StringLength(1024, ErrorMessage = "Email can't be longer than 1024 characters")]
        [MinLength(8, ErrorMessage = "Password Should be at least 8 characters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(1024, ErrorMessage = "Password can't be no longer than 1024 characters")]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*", ErrorMessage = "Password must be a combination of upper case, lower case, number, and at least 8 characters long")]
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
