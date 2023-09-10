using Transaction.Infrastructure.Host.UserClient.Models;

namespace Transaction.Infrastructure.UserClient;

public interface IUserClient
{
    Task<UserCTO> GetUser(string Id);
}