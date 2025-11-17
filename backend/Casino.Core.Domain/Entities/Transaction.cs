using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class Transaction : Entity<Guid>
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public string Type { get; private set; } // Deposit, Withdrawal, Bet, Win
        public DateTime CreatedAt { get; private set; }

        private Transaction() { }
        public Transaction(Guid walletId, decimal amount, string type)
        {
            WalletId = walletId;
            Amount = amount;
            Type = type;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
