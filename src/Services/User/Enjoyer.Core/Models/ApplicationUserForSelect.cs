using System;
using System.Collections.Generic;

namespace Enjoyer.Core.Models
{
    public class ApplicationUserForSelect
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
