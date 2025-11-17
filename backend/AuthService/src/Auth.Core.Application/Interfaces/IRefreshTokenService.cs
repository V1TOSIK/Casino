using Auth.Core.Application.Models;

namespace Auth.Core.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResult> GenerateRefreshToken(Guid userId,
            string device,
            Guid deviceId,
            string ipAddress,
            Guid? tokenId = null,
            CancellationToken cancellationToken = default);
    }
}
