using Casino.Core.Domain.Exceptions;
using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class PermissionEntity : Entity<Guid>
    {
        public string Name { get; private set; }

        // EF Core constructor
        private PermissionEntity() { }
        public PermissionEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidPermissionNameException($"Invalid permission name: {name}");
            Name = name;
        }
    }
}
