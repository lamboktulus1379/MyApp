using AutoMapper;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Enjoyer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Enjoyer.Infrastructure
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationUserContext _applicationUserContext;
        private IMapper _mapper;

        public ApplicationUserRepository(ApplicationUserContext applicationUserContext, IMapper mapper)
        {
            _applicationUserContext = applicationUserContext;
            _mapper = mapper;
        }
        public ApplicationUser Add(ApplicationUser newApplicationUser)
        {

            _applicationUserContext.ApplicationUsers.Add(newApplicationUser);

            _applicationUserContext.SaveChanges();

            return _applicationUserContext.ApplicationUsers.Find(newApplicationUser.Id);
        }

        public ApplicationUser PatchApplicationUserRoles(string email, ApplicationUserForRoles applicationUserForRoles)
        {
            ApplicationUser applicationUser = GetApplicationUserAndRolesByEmail(email);

            foreach (var role in applicationUser.Roles.ToList())
            {
                if (!applicationUserForRoles.Roles.Contains(role.Id))
                {
                    applicationUser.Roles.Remove(role);
                }
            }

            foreach (var role in applicationUserForRoles.Roles)
            {
                if (!applicationUser.Roles.Any(r => r.Id == role))
                {
                    var newRole = new Role { Id = role };
                    _applicationUserContext.Roles.Attach(newRole);
                    applicationUser.Roles.Add(newRole);
                }
            }
            _applicationUserContext.SaveChanges();

            return applicationUser;
        }

        public void Delete(Guid id)
        {
            var existingApplicationuser = _applicationUserContext.ApplicationUsers.Find(id);
            _applicationUserContext.ApplicationUsers.Remove(existingApplicationuser);
        }

        public ApplicationUser GetApplicationUserByEmail(string email)
        {
            return _applicationUserContext.ApplicationUsers.Where(applicationUser => applicationUser.Email.Equals(email)).FirstOrDefault();
        }

        public ApplicationUser GetApplicationUserByEmailAndPassword(string email, string password)
        {
            return _applicationUserContext.ApplicationUsers.Where(applicationUser => applicationUser.Email.Equals(email) && applicationUser.Password.Equals(password)).FirstOrDefault();
        }

        public IEnumerable<ApplicationUser> GetApplicationUsers()
        {
            return _applicationUserContext.ApplicationUsers.ToList();
        }

        public ApplicationUser GetById(Guid Id)
        {
            return _applicationUserContext.ApplicationUsers.Where(applicationUser => applicationUser.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationUser Update(Guid Id, ApplicationUser newApplicationUser)
        {
            var applicationUser = _applicationUserContext.ApplicationUsers.Where(au => au.Id.Equals(Id)).FirstOrDefault<ApplicationUser>();

            if (applicationUser != null)
            {
                applicationUser.FirstName = newApplicationUser.FirstName;
                applicationUser.LastName = newApplicationUser.LastName;
                applicationUser.Gender = newApplicationUser.Gender;
                applicationUser.DateOfBirth = newApplicationUser.DateOfBirth;
                applicationUser.Email = newApplicationUser.Email;
                applicationUser.RefreshToken = newApplicationUser.RefreshToken;
                applicationUser.RefreshTokenExpiry = newApplicationUser.RefreshTokenExpiry;

                _applicationUserContext.SaveChanges();
            }

            return applicationUser;
        }

        public ApplicationUser GetApplicationUserAndRolesByEmail(string email)
        {
            return _applicationUserContext.ApplicationUsers.Include(u => u.Roles).Where(applicationUser => applicationUser.Email.Equals(email)).FirstOrDefault();
        }

        public ApplicationUser GetApplicationUserAndRolesByEmailAndPassword(string email, string password)
        {
            return _applicationUserContext.ApplicationUsers.Include(u => u.Roles).Where(applicationUser => applicationUser.Email.Equals(email) && applicationUser.Password.Equals(password)).FirstOrDefault();

        }
    }
}
