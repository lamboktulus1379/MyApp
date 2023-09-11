using System;
using System.Collections.Generic;

namespace Auth.Core.Models
{
    public class RoleForCreation
    {
        public string Name { get; set; }
        public bool Status { get; set; } = true;
    }
}
