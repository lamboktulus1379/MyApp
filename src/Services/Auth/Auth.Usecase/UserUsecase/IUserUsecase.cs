using Auth.Core.Models;

namespace Auth.Usecase.UserUsecase;


public interface IUserUsecase
{
    public void UpdateBalance(UserBalanceForCreation userBalanceForCreation);
}