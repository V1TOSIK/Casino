using Auth.Core.Application.Interfaces;
using Auth.Core.Application.Models;
using Auth.Core.Application.Ports;
using Auth.Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Auth.Core.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private ILogger<RefreshTokenService> _logger;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, ILogger<RefreshTokenService> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
        }

        public async Task<RefreshTokenResult> GenerateRefreshToken(Guid userId,
            string device,
            Guid deviceId,
            string ipAddress,
            Guid? tokenId = null,
            CancellationToken cancellationToken = default)
        {
            deviceId = deviceId == Guid.Empty ? Guid.NewGuid() : deviceId;

            if (deviceId != Guid.Empty)
                await _refreshTokenRepository.RevokeByDeviceIdAsync(deviceId, cancellationToken);

            var refreshToken = RefreshToken.Create(userId, device, deviceId, ipAddress, tokenId);
            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            _logger.LogInformation("[Auth Service(RefreshTokenService)] New refresh token created for user with ID {userId}.", userId);
            return new RefreshTokenResult(refreshToken.Token, refreshToken.ExpirationDate, refreshToken.DeviceId);
        }
    }
}
