using Auth.Core.Models;

namespace Auth.Core.Interfaces
{
    public interface IApplicationUserRepository
    {
        IEnumerable<ApplicationUser> GetApplicationUsers();
        ApplicationUser Add(ApplicationUser newApplicationUser);
        ApplicationUser GetById(Guid Id);
        ApplicationUser GetApplicationUserByEmail(string email);
        ApplicationUser GetApplicationUserAndRolesByEmail(string email);
        ApplicationUser GetApplicationUserByEmailAndPassword(string email, string password);
        ApplicationUser GetApplicationUserAndRolesByEmailAndPassword(string email, string password);
        ApplicationUser Update(Guid Id, ApplicationUser newApplicationUser);
        ApplicationUser PatchApplicationUserRoles(string email, ApplicationUserForRoles applicationUserForRoles);
        void Delete(Guid id);
    }
}
