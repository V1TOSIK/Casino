using Auth.Adapters.Outbound.Common.Options;
using Auth.Core.Application.Ports;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Auth.Adapters.Outbound.Common.Services
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;
        private readonly IAuthRepository _authRepository;
        public JwtProvider(IOptions<JwtOptions> options, IAuthRepository authRepository)
        {
            _options = options.Value;
            _authRepository = authRepository;
        }

        public async Task<string> GenerateAccessToken(Guid userId, string role, CancellationToken cancellationToken)
        {
            var permissions = await _authRepository.GetPermissionsByRoleNameAsync(role, cancellationToken);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Role, role),
                new("permissions", JsonSerializer.Serialize(permissions))
            ];

            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_options.AccessTokenExpirationTime));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
