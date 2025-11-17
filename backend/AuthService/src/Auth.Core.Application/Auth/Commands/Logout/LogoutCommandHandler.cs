using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.CurrentUser;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;
using SharedKernel.Statics;

namespace Auth.Core.Application.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly ILogger<LogoutCommandHandler> _logger;
        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository,
            ICurrentUserService currentUserService,
            IAuthUnitOfWork unitOfWork,
            ILogger<LogoutCommandHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("LogoutCommandHandler: Current user ID is null.");
                return Result.Failure("Current user ID is null.", ErrorCode.Forbidden);
            }

            var token = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, cancellationToken);
            if (token == null || token.IsRevoked)
                return Result.Failure("Refresh token not found or already revoked.", ErrorCode.NotFound);

            if (token.UserId != userId)
            {
                _logger.LogWarning("LogoutCommandHandler: User {UserId} attempted to revoke a token that does not belong to them.", userId);
                return Result.Failure("You do not have permission to revoke this token.", ErrorCode.Forbidden);
            }

            token.Revoke();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} logged out. Token {Token} revoked.", userId, command.RefreshToken);
            return Result.Success();
        }

    }
}
