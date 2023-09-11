using System;
using System.Collections.Generic;

namespace Auth.Core.Models
{
    public class RoleForUpdate
    {
        public string Name { get; set; }
        public bool Status { get; set; } = true;

        public DateTime UpdatedAt { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
