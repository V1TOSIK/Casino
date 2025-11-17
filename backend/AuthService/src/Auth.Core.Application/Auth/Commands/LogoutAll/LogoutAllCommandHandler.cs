using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.CurrentUser;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;
using SharedKernel.Statics;

namespace Auth.Core.Application.Auth.Commands.LogoutAll
{
    public class LogoutAllCommandHandler : IRequestHandler<LogoutAllCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly ILogger<LogoutAllCommandHandler> _logger;

        public LogoutAllCommandHandler(IRefreshTokenRepository refreshTokenRepository,
            ICurrentUserService currentUserService,
            IAuthUnitOfWork unitOfWork,
            ILogger<LogoutAllCommandHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(LogoutAllCommand command, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            if (currentUserId == Guid.Empty)
                return Result.Failure("User must be authenticated.", ErrorCode.Forbidden);

            var isSelfLogout = command.UserId == null || command.UserId == currentUserId;

            if (!isSelfLogout)
            {
                if (_currentUserService.Role != RoleType.Admin)
                {
                    _logger.LogWarning("User {UserId} attempted to logout another user {TargetUserId} without admin rights.", currentUserId, command.UserId);
                    return Result.Failure("Only admins can logout other users.", ErrorCode.Forbidden);
                }
            }

            await _refreshTokenRepository.RevokeAllAsync(command.UserId ?? currentUserId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("[Auth Service(LogoutAllCommandHandler)] All devices logged out for user with ID {userId}.", command.UserId);
            return Result.Success();
        }
    }
}
