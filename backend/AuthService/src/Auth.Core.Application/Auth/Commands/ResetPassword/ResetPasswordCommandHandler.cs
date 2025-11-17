using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.RedisStorage;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;
using SharedKernel.Statics;

namespace Auth.Core.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRedisStore _redisStore;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(
            IAuthRepository authRepository,
            IRedisStore redisStore,
            IAuthUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ILogger<ResetPasswordCommandHandler> logger)
        {
            _authRepository = authRepository;
            _redisStore = redisStore;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var redisKey = $"reset_token:{command.ResetToken}";
            var userIdString = await _redisStore.GetAsync(RedisKeys.ResetToken(command.ResetToken));
            if (string.IsNullOrEmpty(userIdString))
                return Result.Failure("Invalid or expired reset token.", ErrorCode.Validation);

            var userId = Guid.Parse(userIdString);

            var user = await _authRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("Reset requested for non-existing userId: {userId}", userId);
                return Result.Failure("Invalid or expired reset token.", ErrorCode.Unauthorized);
            }

            var hashedPassword = _passwordHasher.HashPassword(command.NewPassword);

            user.UpdatePassword(hashedPassword);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _redisStore.DeleteAsync(RedisKeys.ResetToken(command.ResetToken));
            _logger.LogInformation("Password reset successfully for user ID: {UserId}", user.Id);
            return Result.Success();

        }
    }
}
