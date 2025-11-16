using Casino.Core.Domain.Exceptions;
using SharedKernel.Domain.Entity;
using System.Data;
using System.Xml.Linq;

namespace Casino.Core.Domain.Entities
{
    public class UserEntity : Entity<Guid>
    {

        public string Email { get; private set; }
        public Guid RoleId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsBanned { get; private set; }
        public DateTime? BannedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // EF Core constructor
        private UserEntity() { }
        private UserEntity(string email, Guid roleId)
        {
            Email = email;
            RoleId = roleId;
            CreatedAt = DateTime.UtcNow;
            IsBanned = false;
            BannedAt = null;
            IsDeleted = false;
            DeletedAt = null;
        }

        public static UserEntity Create(string email, Guid roleId)
        {
            return new UserEntity(email, roleId);
        }

        public void BanUser()
        {
            if (IsBanned)
                throw new UserAlreadyBannedException($"User already banned.");
            IsBanned = true;
            BannedAt = DateTime.UtcNow;
        }

        public void DeleteUser()
        {
            if (IsDeleted)
                throw new UserAlreadyDeletedException($"User already deleted.");
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void UnbanUser()
        {   
            if (IsBanned)
                throw new UserNotBannedException($"User is not banned.");
            IsBanned = false;
            BannedAt = null;
        }

        public void RestoreUser()
        {
            if (!IsDeleted)
                throw new UserNotDeletedException($"User is not deleted.");
            IsDeleted = false;
            DeletedAt = null;
        }   
    }
}
