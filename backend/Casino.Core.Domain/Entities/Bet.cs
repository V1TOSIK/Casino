using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class Bet : Entity<Guid>
    {
        public Guid UserId { get; private set; }
        public decimal Amount { get; private set; }
        public decimal Odds { get; private set; }
        public DateTime PlacedAt { get; private set; }
        public bool IsSettled { get; private set; }
        public string Status { get; private set; }
        public decimal Payout { get; private set; }
        public string GameType { get; private set; }
       
        // EF Core constructor
        private Bet() { }
        public Bet(Guid userId, decimal amount, decimal odds)
        {
            UserId = userId;
            Amount = amount;
            Odds = odds;
            PlacedAt = DateTime.UtcNow;
            IsSettled = false;
            Status = string.Empty;
            Payout = 0m;
        }
        public void SettleBet(string isWon)
        {
            IsSettled = true;
            Status = isWon;
            Payout = isWon != string.Empty ? Amount * Odds : 0m;
        }
    }
}
