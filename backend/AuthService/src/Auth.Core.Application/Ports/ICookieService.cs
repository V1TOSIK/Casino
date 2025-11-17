using Auth.Core.Application.Models;

namespace Auth.Core.Application.Ports
{
    public interface ICookieService
    {
        void Set(string key, string value, DateTime expirationTime);
        string? Get(string key);
        void Delete(string key);
        void SetRefreshTokenCookieIfNotNull(string token, DateTime expiration);
        void SetDeviceIdCookieIfNotNull(Guid? deviceId);
        void SetRefreshTokenAndDeviceCookie(AuthResult result, Guid deviceId);
        void SetResetTokenCookie(string token, DateTime expiration);
        void SetSessionTokenCookie(string token, DateTime expiration);
        string? GetRefreshToken();
        string? GetResetToken();
        string? GetSessionToken();
        Guid GetDeviceId();
        void DeleteRefreshTokenCookie();
        void DeleteResetTokenCookie();
        void DeleteSessionTokenCookie();
    }
}
