using Auth.Core.Domain.Entities;

namespace Auth.Core.Application.Models
{
    public class AuthResult
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime Expiration { get; }
        public Guid? DeviceId { get; }

        public AuthResult(string accessToken,
            RefreshTokenResult refreshTokenResult)
        {
            AccessToken = accessToken;
            RefreshToken = refreshTokenResult.Token;
            Expiration = refreshTokenResult.Expiration;
            DeviceId = refreshTokenResult.DeviceId;
        }
    }
}
