using Auth.Core.Application.Ports;
using Auth.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auth.Adapters.Outbound.PostgresEfWriteAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly PostgresEfWriteAccessDbContext _dbContext;
        private readonly ILogger<RefreshTokenRepository> _logger;
        public RefreshTokenRepository(PostgresEfWriteAccessDbContext dbContext,
            ILogger<RefreshTokenRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpirationDate > DateTime.UtcNow, cancellationToken);

            return refreshToken;
        }

        public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken)
        {
            await _dbContext.RefreshTokens.AddAsync(token, cancellationToken);
        }

        public async Task RevokeAllAsync(Guid userId, CancellationToken cancellationToken)
        {
            await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(rt => rt.IsRevoked, true)
                .SetProperty(rt => rt.RevokedAt, DateTime.UtcNow),
                cancellationToken
            );
        }

        public async Task RevokeByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken)
        {
            var affectedRows = await _dbContext.RefreshTokens
                .Where(rt => rt.DeviceId == deviceId && !rt.IsRevoked)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(rt => rt.IsRevoked, true)
                    .SetProperty(rt => rt.RevokedAt, DateTime.UtcNow),
                    cancellationToken
                );
        }

        public async Task DeleteExpiredAsync(CancellationToken cancellationToken)
        {
            await _dbContext.RefreshTokens
                .Where(rt => rt.ExpirationDate <= DateTime.UtcNow)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
