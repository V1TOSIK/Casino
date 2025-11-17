using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.CurrentUser;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Core.Application.Auth.Commands.SetEmail
{
    public class SetEmailCommandHandler : IRequestHandler<SetEmailCommand, Result>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly ILogger<SetEmailCommandHandler> _logger;
        public SetEmailCommandHandler(IAuthRepository authRepository,
            ICurrentUserService currentUserService,
            IAuthUnitOfWork unitOfWork,
            ILogger<SetEmailCommandHandler> logger)
        {
            _authRepository = authRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(SetEmailCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("[Auth Service(AddEmailCommandHandler)] User ID is null.");
                return Result.Failure("Current user ID is null.", ErrorCode.Forbidden);
            }

            var user = await _authRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("[Auth Service(AddEmailCommandHandler)] User with ID {UserId} not found.", userId);
                return Result.Failure("User not found.", ErrorCode.NotFound);
            }

            if (await _authRepository.IsEmailRegisteredAsync(command.Email, cancellationToken))
            {
                _logger.LogWarning("[Auth Service(AddEmailCommandHandler)] Email {Email} is already registered.", command.Email);
                return Result.Failure($"Email {command.Email} is already registered.", ErrorCode.Conflict);
            }
            user.SetEmail(command.Email);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("[Auth Service(AddEmailCommandHandler)] Email {Email} added for user with ID {UserId}.", command.Email, userId);
            return Result.Success();
        }
    }
}
