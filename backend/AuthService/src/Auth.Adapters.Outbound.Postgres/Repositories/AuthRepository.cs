using Auth.Core.Application.Ports;
using Auth.Core.Domain.Entities;
using Auth.Core.Domain.Enums;
using Auth.Core.Domain.Exceptions;
using Auth.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Regexs;

namespace Auth.Adapters.Outbound.Postgres.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthDbContext _dbContext;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(AuthDbContext dbContext,
            ILogger<AuthRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<UserEntity?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
            => await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        public async Task<UserEntity?> GetByOAuthAsync(string providerUserId, string providerText, CancellationToken cancellationToken)
        {
            var provider = ParseProvider(providerText);
            var oAuthUser = await _dbContext.ExternalLogins
                .FirstOrDefaultAsync(u => u.ProviderUserId == providerUserId
                    && u.Provider == provider, cancellationToken);

            if (oAuthUser == null)
                return null;

            var user = await _dbContext.Users
                .Include(u => u.ExternalLogins)
                .FirstOrDefaultAsync(u => u.Id == oAuthUser.UserId, cancellationToken);

            return user;
        }

        public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var emailValue = new Email(email);

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email != null
                    && u.Email.Equals(emailValue),
                    cancellationToken);

            return user;
        }

        public async Task<UserEntity?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            var phoneNumberValue = new PhoneNumber(phoneNumber);

            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber != null
                    && u.PhoneNumber.Equals(phoneNumberValue),
                    cancellationToken);
        }

        public async Task<UserEntity?> GetByCredentialAsync(string credential, CancellationToken cancellationToken)
        {
            if (RegexPatterns.Email.IsMatch(credential))
                return await GetByEmailAsync(credential, cancellationToken);

            if (RegexPatterns.Phone.IsMatch(credential))
                return await GetByPhoneNumberAsync(credential, cancellationToken);

            throw new InvalidCredentialTypeException($"Credential '{credential}' is not a valid email or phone number format.");

        }

        public async Task AddAsync(UserEntity user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetPermissionsByRoleNameAsync(string roleName, CancellationToken cancellationToken)
        {
            var permissions = await _dbContext.Roles
                .Where(r => r.Name == roleName)
                .SelectMany(r => r.Permissions)
                .Select(p => p.Name)
                .ToListAsync(cancellationToken);
            if (permissions.Count == 0)
            {
                _logger.LogError("[Auth Repository] No permissions found for role {roleName}", roleName);
                throw new RoleNotFoundException($"No permissions found for role {roleName}.");
            }
            return permissions;
        }

        public async Task<Guid> GetRoleIdByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var roleId = await _dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => (Guid?)u.RoleId)
                .FirstOrDefaultAsync(cancellationToken);

            if (roleId is null)
            {
                _logger.LogError("[Auth Repository] User with id {userId} not found", userId);
                throw new UserNotFoundException($"User with id {userId} not found.");
            }

            return roleId.Value;
        }

        public async Task<Guid> GetRoleIdByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            var roleId = await _dbContext.Roles
                .Where(r => r.Name == roleName)
                .Select(r => (Guid?)r.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (roleId is null)
            {
                _logger.LogError("[Auth Repository] Role with name {roleName} not found", roleName);
                throw new RoleNotFoundException($"Role with name {roleName} not found.");
            }
            return roleId.Value;
        }

        public async Task<RoleEntity> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
            if (role is null)
            {
                _logger.LogError("[Auth Repository] Role with id {roleId} not found", roleId);
                throw new RoleNotFoundException($"Role with id {roleId} not found.");
            }
            return role;
        }

        public async Task HardDeleteAsync(Guid userId, CancellationToken cancellationToken)
        {
            //replace to service layer
            var user = await GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                _logger.LogError("[Auth Repository] User with id {userId} not found", userId);
                throw new UserNotFoundException($"User with id {userId} not found.");
            }

            _dbContext.Users.Remove(user);
        }

        public async Task<bool> IsEmailRegisteredAsync(string email, CancellationToken cancellationToken)
        {
            var emailValue = new Email(email);

            return await _dbContext.Users
                .AnyAsync(u => u.Email != null && u.Email.Equals(emailValue), cancellationToken);
        }

        public async Task<bool> IsPhoneNumberRegisteredAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            var phoneNumberValue = new PhoneNumber(phoneNumber);

            return await _dbContext.Users
                .AnyAsync(u => u.PhoneNumber != null && u.PhoneNumber.Equals(phoneNumberValue), cancellationToken);
        }

        private AuthProvider ParseProvider(string providerText)
        {
            if (!Enum.TryParse<AuthProvider>(providerText, true, out AuthProvider provider))
            {
                _logger.LogError("[Auth Module(Repository)] Invalid provider name: {providerText}", providerText);
                throw new InvalidProviderException("Invalid provider name");
            }
            return provider;
        }
    }
}
