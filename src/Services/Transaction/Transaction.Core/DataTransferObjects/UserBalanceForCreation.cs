namespace Transaction.Core.DataTransferObjects;
public class UserBalanceForCreation
{
    public string UserId { get; set; }
    public string TransactionId { get; set; }
    public double Amount { get; set; }
    public int TransactionType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}