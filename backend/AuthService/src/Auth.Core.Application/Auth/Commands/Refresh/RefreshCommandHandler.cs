using Auth.Core.Application.Interfaces;
using Auth.Core.Application.Models;
using Auth.Core.Application.Ports;
using Auth.Core.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Core.Application.Auth.Commands.Refresh
{
    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, TResult<AuthResult>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly IAuthFacade _authFacade;
        private readonly ILogger<RefreshCommandHandler> _logger;

        public RefreshCommandHandler(
            IAuthRepository authRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAuthUnitOfWork unitOfWork,
            IAuthFacade authFacade,
            ILogger<RefreshCommandHandler> logger)
        {
            _authRepository = authRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _authFacade = authFacade;
            _logger = logger;
        }
        public async Task<TResult<AuthResult>> Handle(RefreshCommand command, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, cancellationToken);

            if (token == null || token.IsRevoked)
            {
                _logger.LogWarning("[Auth Service(RefreshCommandHandler)] Refresh token {token} not found or already revoked.", command.RefreshToken);
                return TResult<AuthResult>.Failure("Refresh token not found or already revoked.", ErrorCode.NotFound);
            }

            var user = await _authRepository.GetByIdAsync(token.UserId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("[Auth Service(RefreshCommandHandler)] User with ID {UserId} not found.", token.UserId);
                return TResult<AuthResult>.Failure("User not found.", ErrorCode.NotFound);
            }
            TResult<AuthResult> result = TResult<AuthResult>.Failure("error", ErrorCode.Unknown);
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                token.Revoke();
                result = await _authFacade.BuildAuthResult(user, command.DeviceId, token.Id, cancellationToken);

                if (result.IsSuccess)
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                else
                    throw new BuildAuthResultException(result.Error!.Message);
            }, cancellationToken);

            _logger.LogInformation("[Auth Service(RefreshCommandHandler)] Refresh token for user with ID {userId} refreshed successfully.", user.Id);
            return result;
        }
    }
}
