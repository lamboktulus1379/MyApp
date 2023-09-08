namespace Wallet.Core.Models;

public class UserBalanceLog
{
    public int Id { get; set; }
    public UserBalance UserBalance { get; set; }
    public int TransactionType { get; set; }
    public double Amount { get; set; }
    public double CurrentBalance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}