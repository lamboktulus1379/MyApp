using AutoMapper;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Enjoyer.Infrastructure.Data;

namespace Enjoyer.Infrastructure
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationUserContext _applicationUserContext;
        private readonly IMapper _mapper;

        public RoleRepository(ApplicationUserContext roleContext, IMapper mapper)
        {
            _applicationUserContext = roleContext;
            _mapper = mapper;
        }
        public Role Add(Role newRole)
        {
            _applicationUserContext.Roles.Add(newRole);
            _applicationUserContext.SaveChanges();

            return _applicationUserContext.Roles.Find(newRole.Id);
        }

        public void Delete(Guid id)
        {
            var existingApplicationuser = _applicationUserContext.Roles.Find(id);
            _applicationUserContext.Roles.Remove(existingApplicationuser);
        }

        public IEnumerable<Role> GetRoles()
        {
            return _applicationUserContext.Roles.ToList();
        }

        public Role GetById(Guid Id)
        {
            return _applicationUserContext.Roles.Where(role => role.Id.Equals(Id)).FirstOrDefault();
        }

        public Role Update(Guid Id, Role newRole)
        {
            var role = _applicationUserContext.Roles.Where(r => r.Id.Equals(Id)).FirstOrDefault<Role>();

            if (role != null)
            {
                role.Name = newRole.Name;
                role.Status = newRole.Status;
                role.UpdatedAt = DateTime.Now;

                _applicationUserContext.SaveChanges();
            }

            return role;
        }

        public Role GetByName(string Name)
        {
            return _applicationUserContext.Roles.FirstOrDefault(r => r.Name.Equals(Name));
        }
    }
}
