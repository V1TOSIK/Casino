using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class UserEntity : Entity<Guid>
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public Guid RoleId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        
        // EF Core constructor
        private UserEntity() { }
        private UserEntity(string username, string email, Guid roleId)
        {
            Username = username;
            Email = email;
            RoleId = roleId;
            CreatedAt = DateTime.UtcNow;
        }

        public static UserEntity Create(string username, string email, Guid roleId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));

            return new UserEntity(username, email, roleId);
        }
    }
}
