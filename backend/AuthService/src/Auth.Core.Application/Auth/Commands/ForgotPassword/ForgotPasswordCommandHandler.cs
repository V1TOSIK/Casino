using Auth.Core.Application.Models;
using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.RedisStorage;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;
using SharedKernel.Statics;

namespace Auth.Core.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, TResult<ForgotPasswordResult>>
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IAuthRepository _authRepository;
        private readonly IRedisStore _redisStore;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(
            IVerificationCodeService verificationCodeService,
            IAuthRepository authRepository,
            IRedisStore redisStore,
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _verificationCodeService = verificationCodeService;
            _authRepository = authRepository;
            _redisStore = redisStore;
            _logger = logger;
        }

        public async Task<TResult<ForgotPasswordResult>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var verificationCode = _verificationCodeService.GenerateVerificationCode();

            var user = await _authRepository.GetByCredentialAsync(command.Credential, cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("No user found with credential: {Credential}", command.Credential);
                return TResult<ForgotPasswordResult>.Failure("User not found.", ErrorCode.NotFound);
            }

            await _redisStore.SetAsync(
                key: RedisKeys.ForgotPassword(user.Id),
                value: verificationCode,
                expiry: TimeSpan.FromMinutes(5)
            );

            var sessionToken = Guid.NewGuid().ToString();

            await _redisStore.SetAsync(
                key: RedisKeys.ForgotSession(sessionToken),
                value: user.Id.ToString(),
                expiry: TimeSpan.FromMinutes(5)
            );

            var result = await _verificationCodeService.SendVerificationCodeAsync(
                destination: command.Credential,
                verificationCode: verificationCode,
                cancellationToken);

            if (result.IsFailure)
            {
                _logger.LogError("Failed to send verification code to {Credential}: {Error}",
                    command.Credential,
                    result.Error?.Message);
                return TResult<ForgotPasswordResult>.Failure("Failed to send verification code.", result.Error?.Code ?? ErrorCode.Unknown);
            }

            _logger.LogInformation("Verification code sent to {Credential}", command.Credential);
            return TResult<ForgotPasswordResult>.Success(new ForgotPasswordResult(sessionToken));
        }
    }
}
