using Enjoyer.Core.Models;

namespace Enjoyer.Core.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetRoles();
        Role Add(Role newRole);
        Role GetById(Guid Id);

        Role GetByName(string Name);
        Role Update(Guid Id, Role newRole);
        void Delete(Guid id);
    }
}
