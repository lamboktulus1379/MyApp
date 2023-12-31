﻿using System.ComponentModel.DataAnnotations;

namespace Auth.Core.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public ICollection<ApplicationRole>? ApplicationRoles { get; set; }
    }
}
