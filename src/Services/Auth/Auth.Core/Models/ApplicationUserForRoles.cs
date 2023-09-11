using System;
using System.Collections.Generic;

namespace Auth.Core.Models
{
    public class ApplicationUserForRoles
    {
        public ICollection<Guid> ApplicationRoles { get; set; }
    }
}
