using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class Game : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Type { get; private set; } // Slot, Roulette, etc.
        public string Description { get; private set; }


        // EF Core constructor
        private Game() { }
        public Game(string name, string type, string description)
        {
            Name = name;
            Type = type;
            Description = description;
        }
        public static Game Create(string name, string type, string description)
        {
            return new Game(name, type, description);
        }
    }
}
