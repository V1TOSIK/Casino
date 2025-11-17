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

        public static Bet Create(Guid userId, decimal amount, decimal odds)
        {
            return new Bet(userId, amount, odds);
        }

        public void SettleBet(string isWon)
        {
            IsSettled = true;
            Status = isWon;
            Payout = isWon != string.Empty ? Amount * Odds : 0m;
        }

        public void UpdateBet(decimal? amount, decimal? odds, string? gameType, string? status, decimal? payout)
        {
            if (amount.HasValue)
               UpdateAmount(amount.Value);
            if (odds.HasValue)
                UpdateOdds(odds.Value);
            if (!string.IsNullOrEmpty(gameType))
                UpdateGameType(gameType);
            if (!string.IsNullOrEmpty(status))             
                UpdateStatus(status);
            if (payout.HasValue)
                UpdatePayout(payout.Value);
        }
        private void UpdateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Bet amount must be greater than zero.");
            Amount = amount;
        }

        private void UpdateOdds(decimal odds)
        {
            if (odds <= 0)
                throw new ArgumentException("Odds must be greater than zero.");
            Odds = odds;
        }

        private void UpdateGameType(string gameType)
        {
            if (string.IsNullOrWhiteSpace(gameType))
                throw new ArgumentException("Game type cannot be empty.");
            GameType = gameType;
        }

        private void UpdateStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("Status cannot be empty.");
            Status = status;
        }

        private void UpdatePayout(decimal payout)
        {
            if (payout < 0)
                throw new ArgumentException("Payout cannot be negative.");
            Payout = payout;
        }
    }
}
