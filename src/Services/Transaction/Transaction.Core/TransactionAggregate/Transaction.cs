namespace Transaction.Core.TransactionAggregate;

public class Transaction
{
    public Transaction()
    {
        this.Id = Guid.NewGuid().ToString();
    }
    public string Id { get; set; }
    public string UserId { get; set; }
    public string SupplierId { get; set; }
    public string PaymentMethod { get; set; }
    public double Amount { get; set; }
    public string Currency { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}