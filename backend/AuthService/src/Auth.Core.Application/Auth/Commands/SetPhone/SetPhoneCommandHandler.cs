using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.CurrentUser;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Core.Application.Auth.Commands.SetPhone
{
    public class SetPhoneCommandHandler : IRequestHandler<SetPhoneCommand, Result>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly ILogger<SetPhoneCommandHandler> _logger;

        public SetPhoneCommandHandler(IAuthRepository authRepository,
            ICurrentUserService currentUserService,
            IAuthUnitOfWork unitOfWork,
            ILogger<SetPhoneCommandHandler> logger)
        {
            _authRepository = authRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(SetPhoneCommand command, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("[Auth Service(SetPhoneCommandHandler)] SetPhoneCommandHandler: Current user ID is null.");
                return Result.Failure("Current user ID is null.", ErrorCode.Forbidden);
            }

            var user = await _authRepository.GetByIdAsync(userId, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("[Auth Service(SetPhoneCommandHandler)] SetPhoneCommandHandler: User with ID {UserId} not found.", userId);
                return Result.Failure("User not found.", ErrorCode.NotFound);
            }

            user.SetPhone(command.PhoneNumber);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("[Auth Service(SetPhoneCommandHandler)]SetPhoneCommandHandler: Phone number set successfully for user with ID {UserId}.", userId);
            return Result.Success();
        }
    }
}
