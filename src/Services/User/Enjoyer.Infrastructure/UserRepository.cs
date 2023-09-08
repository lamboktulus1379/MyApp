using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Enjoyer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Enjoyer.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationUserContext _applicationUserContext;

        public UserRepository(ApplicationUserContext applicationUserContext)
        {
            _applicationUserContext = applicationUserContext;
        }
        public User Add(User newUser)
        {

            _applicationUserContext.Users.Add(newUser);

            _applicationUserContext.SaveChanges();

            return _applicationUserContext.Users.Find(newUser.Id);
        }

        public void Delete(Guid id)
        {
            var existingApplicationuser = _applicationUserContext.Users.Find(id);
            _applicationUserContext.Users.Remove(existingApplicationuser);
        }

        public User GetUserByEmail(string email)
        {
            return _applicationUserContext.Users.Where(applicationUser => applicationUser.Email.Equals(email)).FirstOrDefault();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _applicationUserContext.Users.ToListAsync();
        }

        public User GetById(string Id)
        {
            return _applicationUserContext.Users.Where(applicationUser => applicationUser.Id.Equals(Id)).FirstOrDefault();
        }

        public User Update(string Id, User newUser)
        {
            var applicationUser = _applicationUserContext.Users.Where(au => au.Id.Equals(Id)).FirstOrDefault<User>();

            if (applicationUser != null)
            {
                applicationUser.FirstName = newUser.FirstName;
                applicationUser.LastName = newUser.LastName;
                applicationUser.Gender = newUser.Gender;
                applicationUser.DateOfBirth = newUser.DateOfBirth;
                applicationUser.Email = newUser.Email;
                applicationUser.RefreshToken = newUser.RefreshToken;
                applicationUser.RefreshTokenExpiry = newUser.RefreshTokenExpiry;

                _applicationUserContext.SaveChanges();
            }

            return applicationUser;
        }
    }
}
