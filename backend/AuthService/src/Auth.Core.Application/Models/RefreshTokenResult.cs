namespace Auth.Core.Application.Models
{
    public class RefreshTokenResult
    {
        public string Token { get; }
        public DateTime Expiration { get; }
        public Guid? DeviceId { get; }

        public RefreshTokenResult(string token, DateTime expiration, Guid? deviceId)
        {
            Token = token;
            Expiration = expiration;
            DeviceId = deviceId;
        }
    }
}
