
using Auth.Core.Interfaces;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using Auth.Core.Models;

namespace Auth.Usecase.UserUsecase;

public class UserUsecase : IUserUsecase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserUsecase> _logger;
    public UserUsecase(IUserRepository userRepository, ILogger<UserUsecase> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public void UpdateBalance(UserBalanceForCreation userBalanceForCreation)
    {
        var user = _userRepository.GetById(userBalanceForCreation.UserId);

        if (user == null)
        {
            return;
        }

        user.Balance = user.Balance + userBalanceForCreation.Balance;

        user = _userRepository.Update(user.Id, user);
        _logger.LogInformation("User: {0}", user);
    }
}

