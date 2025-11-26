using SharedKernel.Domain.AggregateRoot;
using User.Core.Domain.Exceptions;

namespace User.Core.Domain.Entities
{
    public class UserEntity : AggregateRoot<Guid>
    {
        // For EF
        private UserEntity() { }

        private UserEntity(Guid id)
        {
            Id = id;
        }

        public bool IsDeleted { get; private set; }
        public bool IsBanned { get; private set; }

        public UserEntity Create(Guid id)
        {
            if (id == Guid.Empty)
                throw new InvalidUserIdException("User id is empty");

            return new UserEntity(id);
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void Ban()
        {
            IsBanned = true;
        }
    }
}
