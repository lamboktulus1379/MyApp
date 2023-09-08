using Wallet.Core.Models;

namespace Wallet.Core.DataTransferObjects;

public class UserBalanceLogForCreation
{
    public UserBalance UserBalance { get; set; }
    public int TransactionType { get; set; }
    public double Amount { get; set; }
    public double CurrentBalance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}