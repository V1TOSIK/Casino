using Auth.Core.Domain.Exceptions;
using SharedKernel.Domain.AggregateRoot;
using System.Security.Cryptography;

namespace Auth.Core.Domain.Entities
{
    public class RefreshToken : AggregateRoot<Guid>
    {
        public const byte EXPIRATION_DAYS = 10;
        private RefreshToken(Guid userId, string device, Guid deviceId, string ipAddress, Guid? replacedByTokenId)
        {
            UserId = userId;
            ExpirationDate = DateTime.UtcNow.AddDays(EXPIRATION_DAYS);
            ReplacedByTokenId = replacedByTokenId;
            Device = device;
            DeviceId = deviceId;
            IpAddress = ipAddress;

            Token = GenerateToken();
        }

        public Guid UserId { get; private set; }
        public string Token { get; }
        public DateTime ExpirationDate { get; private set; }
        public bool IsRevoked { get; private set; } = false;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; private set; }

        public Guid? ReplacedByTokenId { get; set; }
        public string Device { get; private set; }
        public Guid DeviceId { get; private set; }
        public string IpAddress { get; private set; }

        public static RefreshToken Create(Guid userId, string device, Guid deviceId, string ipAddress, Guid? replacedByTokenId = null)
        {
            if (userId == Guid.Empty) throw new EmptyUserIdException("User ID cannot be empty.");

            if (string.IsNullOrWhiteSpace(device)) throw new NullableDeviceException("Device cannot be empty");

            if (deviceId == Guid.Empty) throw new EmptyDeviceIdException("DeviceId cannot be empty");

            if (string.IsNullOrWhiteSpace(ipAddress)) throw new NullableIpAddressException("IpAddress cannot be empty");

            return new RefreshToken(userId, device, deviceId, ipAddress, replacedByTokenId);
        }

        public void Revoke()
        {
            if (IsRevoked)
                throw new TokenAlreadyRevokedException("Token is already revoked.");

            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }

        private string GenerateToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
