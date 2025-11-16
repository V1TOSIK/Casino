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
    }
}
