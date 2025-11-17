using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.CurrentUser;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Core.Application.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;
        public ChangePasswordCommandHandler(IAuthRepository authRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ICurrentUserService currentUserService,
            IPasswordHasher passwordHasher,
            IAuthUnitOfWork unitOfWork,
            ILogger<ChangePasswordCommandHandler> logger)
        {
            _authRepository = authRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _currentUserService = currentUserService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("[Auth Service(ChangePasswordCommandHandler)] Current user ID is null.");
                return Result.Failure("Current user ID is null.", ErrorCode.Forbidden);
            }

            var user = await _authRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("[Auth Service(ChangePasswordCommandHandler)] User with ID {userId} not found.", userId);
                return Result.Failure("User not found.", ErrorCode.NotFound);
            }

            user.EnsureActive();

            if (user.Password == null)
            {
                _logger.LogWarning("[Auth Service(ChangePasswordCommandHandler)] User with ID {userId} has no password set.", user.Id);
                return Result.Failure("User has no password set.", ErrorCode.Conflict);
            }

            if (!_passwordHasher.VerifyHashedPassword(command.CurrentPassword, user.Password.Value))
            {
                _logger.LogWarning("[Auth Service(ChangePasswordCommandHandler)] Invalid password attempt for user with ID {userId}.", user.Id);
                return Result.Failure("Invalid password.", ErrorCode.Conflict);
            }

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var newHashedPassword = _passwordHasher.HashPassword(command.NewPassword);
                user.UpdatePassword(newHashedPassword);

                await _refreshTokenRepository.RevokeAllAsync(user.Id, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }, cancellationToken);


            _logger.LogInformation("[Auth Service(ChangePasswordCommandHanlder)] Password changed successfully for user with ID {userId}.", user.Id);
            return Result.Success();
        }
    }
}
