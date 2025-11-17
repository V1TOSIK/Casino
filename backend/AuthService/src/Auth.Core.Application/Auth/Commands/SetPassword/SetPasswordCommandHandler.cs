using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.CurrentUser;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Core.Application.Auth.Commands.SetPassword
{
    public class SetPasswordCommandHandler : IRequestHandler<SetPasswordCommand, Result>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<SetPasswordCommandHandler> _logger;
        public SetPasswordCommandHandler(IAuthRepository authRepository,
            ICurrentUserService currentUserService,
            IAuthUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ILogger<SetPasswordCommandHandler> logger)
        {
            _authRepository = authRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<Result> Handle(SetPasswordCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("[Auth Service(SetPasswordCommandHandler)] SetPasswordCommandHandler: Current user ID is null.");
                return Result.Failure("Current user ID is null.", ErrorCode.Forbidden);
            }

            var user = await _authRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("[Auth Service(SetPasswordCommandHandler)] SetPasswordCommandHandler: User with ID {UserId} not found.", userId);
                return Result.Failure("User not found.", ErrorCode.NotFound);
            }


            if (user.Password != null)
            {
                _logger.LogWarning("[Auth Service(SetPasswordCommandHandler)] SetPasswordCommandHandler: User with ID {UserId} already has a password set.", userId);
                return Result.Failure("Password is already set.", ErrorCode.Conflict);
            }

            var hashedPassword = _passwordHasher.HashPassword(command.Password);

            user.SetPassword(hashedPassword);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("[Auth Service(SetPasswordCommandHandler)]SetPasswordCommandHandler: Password set successfully for user with ID {UserId}.", userId);
            return Result.Success();
        }
    }
}
