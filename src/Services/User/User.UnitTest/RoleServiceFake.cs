using System;
using System.Collections.Generic;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;

namespace User.UnitTest
{
    public class RoleServiceFake : IRoleRepository
    {
        private readonly List<Role> _roles;
        public RoleServiceFake()
        {
            _roles = new List<Role> {
                new Role { CreatedAt = DateTime.Now, Id = new Guid(), Name = "User", Status = true, UpdatedAt = DateTime.Now }
            };
        }

        public Role Add(Role newRole)
        {
            _roles.Add(newRole);
            return newRole;
        }


        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Role GetById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Role GetByName(string Name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetRoles()
        {
            throw new NotImplementedException();
        }

        public Role Update(Guid Id, Role newRole)
        {
            throw new NotImplementedException();
        }


        Role IRoleRepository.GetById(Guid Id)
        {
            throw new NotImplementedException();
        }

        Role IRoleRepository.GetByName(string Name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Role> IRoleRepository.GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}
