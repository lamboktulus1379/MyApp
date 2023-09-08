using System;

namespace Wallet.Core.Models
{
    public class UserBalance
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public double PreviousBalance { get; set; }
        public double CurrentBalance { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<UserBalanceLog> UserBalanceLogs { get; set; }

        public UserBalance()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

