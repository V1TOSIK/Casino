using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class GameInstance : Entity<Guid>
    {
        public Guid GameId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }
        public string Result { get; private set; }

        private GameInstance() { }
    }
}
