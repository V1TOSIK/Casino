using User.Core.Domain.Entities;

namespace User.Core.Application.Ports
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
