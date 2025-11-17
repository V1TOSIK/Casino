using Auth.Core.Domain.Entities;
using SharedKernel.Enums;

namespace Auth.Core.Application.Ports
{
    public interface IAuthRepository
    {
        Task<UserEntity?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserEntity?> GetByOAuthAsync(string providerUserId, string provider, CancellationToken cancellationToken);
        Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<UserEntity?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken);
        Task<UserEntity?> GetByCredentialAsync(string credential, CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetPermissionsByRoleNameAsync(string roleName, CancellationToken cancellationToken);
        Task<RoleEntity> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken);
        Task<Guid> GetRoleIdByNameAsync(string roleName, CancellationToken cancellationToken);
        Task AddAsync(UserEntity user, CancellationToken cancellationToken);
        Task HardDeleteAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> IsEmailRegisteredAsync(string email, CancellationToken cancellationToken);
        Task<bool> IsPhoneNumberRegisteredAsync(string phoneNumber, CancellationToken cancellationToken);
    }
}
