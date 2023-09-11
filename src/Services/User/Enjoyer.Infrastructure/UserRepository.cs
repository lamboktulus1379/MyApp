using System.Security.Claims;
using Enjoyer.Core.DataTransferObjects;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Enjoyer.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Enjoyer.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationUserContext _applicationUserContext;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationUserContext applicationUserContext, UserManager<User> userManager)
        {
            _applicationUserContext = applicationUserContext;
            _userManager = userManager;
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

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            return _userManager.Users.Select(c => new UserDto()
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Gender = c.Gender,
                Balance = c.Balance,
                UserName = c.UserName.ToString(),
                Email = c.UserName.ToString(),
                Roles = _userManager.GetRolesAsync(c).Result.ToList()!,
            }).ToList();
        }

        public User GetById(string Id)
        {
            var user = _applicationUserContext.Users.Where(applicationUser => applicationUser.Id.Equals(Id)).FirstOrDefault();

            return user;
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
