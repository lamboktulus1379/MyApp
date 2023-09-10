using Enjoyer.Core.Models;

namespace Enjoyer.Usecase.UserUsecase;


public interface IUserUsecase
{
    public void UpdateBalance(UserBalanceForCreation userBalanceForCreation);
}