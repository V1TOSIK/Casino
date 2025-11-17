using Casino.Core.Domain.Exceptions;
using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class Wallet : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }
        public string Currency { get; private set; }    
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsBanned { get; private set; }
        public DateTime? BannedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // EF Core constructor
        private Wallet() { }   
        public Wallet(Guid userId, string currency)
        {
            UserId = userId;
            Currency = currency;
            Balance = 0m;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
            IsBanned = false;
            BannedAt = null;
            IsDeleted = false;
            DeletedAt = null;
        }

        public static Wallet Create(Guid roleId, string currency)
        {
            return new Wallet(roleId, currency);
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeCurrency(string currency)
        {
            Currency = currency;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Withdraw(decimal amount)
        {
            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }  
        
        public void BanWallet()
        {
            if (IsBanned)
                throw new WalletAlreadyBannedException($"Wallet already banned.");
            IsBanned = true;
            BannedAt = DateTime.UtcNow;
        }

        public void UnbanWallet()
        {
            if (!IsBanned)
                throw new WalletAlreadyNotBannedException($"Wallet is not banned.");
            IsBanned = false;
            BannedAt = null;
        }

        public void DeleteWallet()
        {
            if (IsDeleted)
                throw new WalletAlreadyDeletedException($"Wallet already deleted.");
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void RestoreWallet()
        {
            if (!IsDeleted)
                throw new WalletNotDeletedException($"Wallet is not deleted.");
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
