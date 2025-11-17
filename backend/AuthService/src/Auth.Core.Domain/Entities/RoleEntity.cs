using Auth.Core.Domain.Exceptions;
using SharedKernel.Domain.AggregateRoot;

namespace Auth.Core.Domain.Entities
{
    public class RoleEntity : AggregateRoot<Guid>
    {
        public string Name { get; private set; }

        private readonly List<PermissionEntity> _permissions = new();
        public IReadOnlyCollection<PermissionEntity> Permissions => _permissions.AsReadOnly();

        // EF Core constructor
        private RoleEntity() { }
        public RoleEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidRoleNameException($"Invalid role name: {name}");
            Name = name;
        }

        public void AddPermission(PermissionEntity permission)
        {
            if (!_permissions.Contains(permission))
                _permissions.Add(permission);
        }
    }
}
