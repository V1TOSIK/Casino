using Auth.Core.Application.Ports;
using Auth.Core.Domain.Exceptions;

namespace Auth.Adapters.Outbound.Common.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new NullablePasswordException("Password cannot be null or empty.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        public bool VerifyHashedPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new NullablePasswordException("Password cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new NullablePasswordException("Hashed password cannot be null or empty.");

            var isVerified = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            return isVerified;
        }
    }
}
