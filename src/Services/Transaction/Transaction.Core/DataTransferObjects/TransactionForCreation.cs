namespace Transaction.Core.DataTransferObjects;

public class TransactionForCreation
{
    public string UserId { get; set; }
    public string SupplierId { get; set; }
    public string PaymentMethod { get; set; }
    public double Amount { get; set; }
    public string Currency { get; set; }
}