using Enjoyer.Core.DataTransferObjects;
using Enjoyer.Core.Models;

namespace Enjoyer.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetUsers();
        User Add(User newUser);
        User GetById(string Id);
        User GetUserByEmail(string email);
        User Update(string Id, User newUser);
        void Delete(Guid id);
    }
}
