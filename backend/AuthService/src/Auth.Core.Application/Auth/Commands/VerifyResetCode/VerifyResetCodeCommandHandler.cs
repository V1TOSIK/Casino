using Auth.Core.Application.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.RedisStorage;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;
using SharedKernel.Statics;
using System.Security.Cryptography;

namespace Auth.Core.Application.Auth.Commands.VerifyResetCode
{
    public class VerifyResetCodeCommandHandler : IRequestHandler<VerifyResetCodeCommand, TResult<VerifyResetCodeResult>>
    {
        private readonly IRedisStore _redisStore;
        private readonly ILogger<VerifyResetCodeCommandHandler> _logger;
        public VerifyResetCodeCommandHandler(
            IRedisStore redisStore,
            ILogger<VerifyResetCodeCommandHandler> logger)
        {
            _redisStore = redisStore;
            _logger = logger;
        }

        public async Task<TResult<VerifyResetCodeResult>> Handle(VerifyResetCodeCommand command, CancellationToken cancellationToken)
        {
            var userIdString = await _redisStore.GetAsync(RedisKeys.ForgotSession(command.SessionToken));

            if (userIdString == null)
            {
                _logger.LogWarning("Invalid or expired session token for sessionToken {sessionToken}", command.SessionToken);
                return TResult<VerifyResetCodeResult>.Failure("Invalid or expired session token.", ErrorCode.Unauthorized);
            }

            var userId = Guid.Parse(userIdString ?? string.Empty);

            var storedCode = await _redisStore.GetAsync(RedisKeys.ForgotPassword(userId));
            if (storedCode == null)
            {
                _logger.LogWarning("No verification code found for {userId}", userId);
                return TResult<VerifyResetCodeResult>.Failure("Verification code not found or expired.", ErrorCode.NotFound);
            }
            if (storedCode != command.VerificationCode)
            {
                _logger.LogWarning("Invalid verification code for {userId}", userId);
                return TResult<VerifyResetCodeResult>.Failure("Invalid verification code.", ErrorCode.Conflict);
            }


            var resetToken = GenerateResetToken();

            await _redisStore.SetAsync(
                key: RedisKeys.ResetToken(resetToken),
                value: userId.ToString(),
                expiry: TimeSpan.FromMinutes(10));

            var result = new VerifyResetCodeResult(resetToken);
            await _redisStore.DeleteAsync(RedisKeys.ForgotPassword(userId));
            _logger.LogInformation("Verification code for {userId} verified successfully", userId);
            return TResult<VerifyResetCodeResult>.Success(result);
        }

        private string GenerateResetToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }
    }
}
