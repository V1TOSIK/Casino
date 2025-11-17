using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class Game : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Type { get; private set; } // Slot, Roulette, etc.
        public string Description { get; private set; }
    }
}
