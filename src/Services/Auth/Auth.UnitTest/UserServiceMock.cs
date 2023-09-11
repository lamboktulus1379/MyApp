using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Interfaces;

namespace User.UnitTest
{
    public class UserServiceMock : IUserRepository
    {
        private readonly List<Enjoyer.Core.Models.User> _users;
        public UserServiceMock()
        {
            _users = new List<Enjoyer.Core.Models.User>()
            {
                new Enjoyer.Core.Models.User {Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200").ToString(), FirstName = "Lambok Tulus", LastName = "Simamora", DateOfBirth = new DateTime(1995, 10, 29), Email = "lamboktulus1379@gmail.com", Gender = "M"  },
                 new Enjoyer.Core.Models.User {Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f").ToString(), FirstName = "Jupri", LastName = "Simamora", DateOfBirth = new DateTime(1997, 8, 5), Email = "jupri@gmail.com", Gender = "M"  },
                 new Enjoyer.Core.Models.User {Id = new Guid("e6d2f5af-7bf7-4eb3-ab05-1c531ecc1968").ToString(), FirstName = "Henri", LastName = "Simamora", DateOfBirth = new DateTime(1999, 9, 1), Email = "henri@gmail.com", Gender = "M"  },
            };
        }

        public Enjoyer.Core.Models.User Add(Enjoyer.Core.Models.User newUser)
        {
            newUser.Id = new Guid().ToString();
            _users.Add(newUser);

            return newUser;
        }

        public void Delete(string id)
        {
            var existing = _users.First(user => user.Id == id);
            _users.Remove(existing);
        }

        public Enjoyer.Core.Models.User GetUserAndRolesByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Enjoyer.Core.Models.User GetUserAndRolesByEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Enjoyer.Core.Models.User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Enjoyer.Core.Models.User GetUserByEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enjoyer.Core.Models.User> GetUsers()
        {
            return _users;
        }

        public Enjoyer.Core.Models.User GetById(string Id)
        {
            return _users.Where(user => user.Id == Id).FirstOrDefault();
        }

        public Enjoyer.Core.Models.User PatchUserRoles(string email)
        {
            throw new NotImplementedException();
        }

        public Enjoyer.Core.Models.User Update(string Id, Enjoyer.Core.Models.User newUser)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Enjoyer.Core.Models.User>> IUserRepository.GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
