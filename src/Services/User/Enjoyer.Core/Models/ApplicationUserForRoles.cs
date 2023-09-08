using System;
using System.Collections.Generic;

namespace Enjoyer.Core.Models
{
    public class ApplicationUserForRoles
    {
        public ICollection<Guid> Roles { get; set; }
    }
}
