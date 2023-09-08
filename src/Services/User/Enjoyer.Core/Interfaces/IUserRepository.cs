﻿using Enjoyer.Core.Models;

namespace Enjoyer.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        User Add(User newUser);
        User GetById(string Id);
        User GetUserByEmail(string email);
        User Update(string Id, User newUser);
        void Delete(Guid id);
    }
}
