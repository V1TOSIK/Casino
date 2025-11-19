using Casino.Core.Domain.Exceptions;
using SharedKernel.Domain.Entity;

namespace Casino.Core.Domain.Entities
{
    public class UserEntity : Entity<Guid>
    {
        public string UserName { get; set; }
        public bool IsBanned { get; private set; } = false;
        public bool IsDeleted { get; private set; } = false;

        // EF Core constructor
        private UserEntity() { }
        private UserEntity(string userName)
        {
            UserName = userName;
        }

        public static UserEntity Create(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidUserNameException("User name cannot be null or empty");
            }

            return new UserEntity(userName);
        }

        public void BanUser()
        {
            if (IsBanned)
                throw new UserAlreadyBannedException($"User already banned.");
            IsBanned = true;
        }

        public void DeleteUser()
        {
            if (IsDeleted)
                throw new UserAlreadyDeletedException($"User already deleted.");
            IsDeleted = true;
        }

        public void UnbanUser()
        {   
            if (IsBanned)
                throw new UserNotBannedException($"User is not banned.");
            IsBanned = false;
        }

        public void RestoreUser()
        {
            if (IsDeleted)
                throw new UserNotDeletedException($"User is not deleted.");
            IsDeleted = false;
        }   
    }
}
