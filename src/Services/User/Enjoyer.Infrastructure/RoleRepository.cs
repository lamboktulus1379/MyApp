using AutoMapper;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Enjoyer.Infrastructure.Data;

namespace Enjoyer.Infrastructure
{
    public class ApplicationRoleRepository : IApplicationRoleRepository
    {
        private readonly ApplicationUserContext _applicationUserContext;
        private readonly IMapper _mapper;

        public ApplicationRoleRepository(ApplicationUserContext roleContext, IMapper mapper)
        {
            _applicationUserContext = roleContext;
            _mapper = mapper;
        }
        public ApplicationRole Add(ApplicationRole newApplicationRole)
        {
            _applicationUserContext.ApplicationRoles.Add(newApplicationRole);
            _applicationUserContext.SaveChanges();

            return _applicationUserContext.ApplicationRoles.Find(newApplicationRole.Id);
        }

        public void Delete(Guid id)
        {
            var existingApplicationuser = _applicationUserContext.ApplicationRoles.Find(id);
            _applicationUserContext.ApplicationRoles.Remove(existingApplicationuser);
        }

        public IEnumerable<ApplicationRole> GetApplicationRoles()
        {
            return _applicationUserContext.ApplicationRoles.ToList();
        }

        public ApplicationRole GetById(Guid Id)
        {
            return _applicationUserContext.ApplicationRoles.Where(role => role.Id.Equals(Id)).FirstOrDefault();
        }

        public ApplicationRole Update(Guid Id, ApplicationRole newApplicationRole)
        {
            var role = _applicationUserContext.ApplicationRoles.Where(r => r.Id.Equals(Id)).FirstOrDefault<ApplicationRole>();

            if (role != null)
            {
                role.Name = newApplicationRole.Name;
                role.Status = newApplicationRole.Status;
                role.UpdatedAt = DateTime.Now;

                _applicationUserContext.SaveChanges();
            }

            return role;
        }

        public ApplicationRole GetByName(string Name)
        {
            return _applicationUserContext.ApplicationRoles.FirstOrDefault(r => r.Name.Equals(Name));
        }
    }
}
