using Auth.Core.Application.Interfaces;
using Auth.Core.Application.Models;
using Auth.Core.Application.Ports;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Core.Application.Auth.Commands.Restore
{
    public class RestoreCommandHandler : IRequestHandler<RestoreCommand, TResult<AuthResult>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IAuthFacade _authHelper;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<RestoreCommandHandler> _logger;
        public RestoreCommandHandler(
            IAuthRepository authRepository,
            IAuthFacade authHelper,
            IAuthUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ILogger<RestoreCommandHandler> logger)
        {
            _authRepository = authRepository;
            _authHelper = authHelper;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<TResult<AuthResult>> Handle(RestoreCommand command, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetByCredentialAsync(command.Credential, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("[Auth Service(RestoreCommandHandler)] User with Credential {Credential} not found.", command.Credential);
                return TResult<AuthResult>.Failure("User not found.", ErrorCode.NotFound);
            }

            if (user.Password == null)
            {
                _logger.LogWarning("[Auth Service(RestoreCommandHandler)] User with Credential {Credential} has no password set.", command.Credential);
                return TResult<AuthResult>.Failure("User has no password set.", ErrorCode.Conflict);
            }
            else if (!_passwordHasher.VerifyHashedPassword(command.Password, user.Password))
            {
                _logger.LogWarning("[Auth Service(RestoreCommandHandler)] Invalid security stamp provided for user with Credential {Credential}.", command.Credential);
                return TResult<AuthResult>.Failure("Invalid Password.", ErrorCode.Conflict);
            }

            TResult<AuthResult> response = null!;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                user.Restore();
                response = await _authHelper.BuildAuthResult(user, command.DeviceId, cancellationToken: cancellationToken);
            }, cancellationToken);
            _logger.LogInformation("[Auth Service(RestoreCommandHandler)] User with Credential {Credential} restored successfully.", command.Credential);
            return response;
        }
    }
}
