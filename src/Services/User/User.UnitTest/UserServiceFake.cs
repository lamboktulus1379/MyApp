using System;
using System.Collections.Generic;
using System.Linq;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;

namespace User.UnitTest
{
    public class UserServiceFake : IApplicationUserRepository
    {
        private readonly List<ApplicationUser> _applicationUsers;

        public UserServiceFake()
        {
            _applicationUsers = new List<ApplicationUser>()
            {
                new ApplicationUser {Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"), FirstName = "Lambok Tulus", LastName = "Simamora", DateOfBirth = new DateTime(1995, 10, 29), Email = "lamboktulus1379@gmail.com", Gender = "M"  },
                 new ApplicationUser {Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"), FirstName = "Jupri", LastName = "Simamora", DateOfBirth = new DateTime(1997, 8, 5), Email = "jupri@gmail.com", Gender = "M"  },
                 new ApplicationUser {Id = new Guid("e6d2f5af-7bf7-4eb3-ab05-1c531ecc1968"), FirstName = "Henri", LastName = "Simamora", DateOfBirth = new DateTime(1999, 9, 1), Email = "henri@gmail.com", Gender = "M"  },
            };
        }

        public ApplicationUser Add(ApplicationUser newApplicationUser)
        {
            newApplicationUser.Id = new Guid();
            _applicationUsers.Add(newApplicationUser);

            return newApplicationUser;
        }

        public void Delete(Guid id)
        {
            var existing = _applicationUsers.First(applicationUser => applicationUser.Id == id);
            _applicationUsers.Remove(existing);
        }

        public ApplicationUser GetApplicationUserAndRolesByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetApplicationUserAndRolesByEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetApplicationUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetApplicationUserByEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetApplicationUsers()
        {
            return _applicationUsers;
        }

        public ApplicationUser GetById(Guid Id)
        {
            return _applicationUsers.Where(applicationUser => applicationUser.Id == Id).FirstOrDefault();
        }

        public ApplicationUser PatchApplicationUserRoles(string email, ApplicationUserForRoles applicationUserForRoles)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser Update(Guid Id, ApplicationUser newApplicationUser)
        {
            throw new NotImplementedException();
        }
    }
}
