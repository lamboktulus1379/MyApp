﻿using Enjoyer.Core.Models;

namespace Enjoyer.Core.Interfaces
{
    public interface IApplicationRoleRepository
    {
        IEnumerable<ApplicationRole> GetApplicationRoles();
        ApplicationRole Add(ApplicationRole newApplicationRole);
        ApplicationRole GetById(Guid Id);

        ApplicationRole GetByName(string Name);
        ApplicationRole Update(Guid Id, ApplicationRole newRole);
        void Delete(Guid id);
    }
}
