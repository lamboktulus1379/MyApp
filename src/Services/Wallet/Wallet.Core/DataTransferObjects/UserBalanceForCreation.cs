using Wallet.Core.Models;

namespace Wallet.Core.DataTransferObjects
{
    public class UserBalanceForCreation
    {
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public double PreviousBalance { get; set; }
        public double CurrentBalance { get; set; }
        public double Amount { get; set; }
        public int TransactionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<UserBalanceLog> UserBalanceLogs { get; set; }
    }
}

