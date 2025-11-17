using Auth.Core.Application.Ports;
using Auth.Core.Application.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Auth.Core.Application.Interfaces;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Core.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TResult<AuthResult>>
    {
        private readonly IAuthRepository _authUserRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthFacade _authFacade;
        private readonly ILogger<LoginCommandHandler> _logger;
        public LoginCommandHandler(
            IAuthRepository authUserRepository,
            IPasswordHasher passwordHasher,
            IAuthFacade authFacade,
            ILogger<LoginCommandHandler> logger)
        {
            _authUserRepository = authUserRepository;
            _passwordHasher = passwordHasher;
            _authFacade = authFacade;
            _logger = logger;
        }

        public async Task<TResult<AuthResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _authUserRepository.GetByCredentialAsync(command.Credential, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("[Auth Service(LoginCommandHandler)] User with this credential: {Credential} was not found.", command.Credential);
                return TResult<AuthResult>.Failure("User with this credential was not found.", ErrorCode.NotFound);
            }
            if (user.Password == null)
            {
                _logger.LogWarning("[Auth Service(LoginCommandHandler)] User with this credential: {Credential} has no password set.", command.Credential);
                return TResult<AuthResult>.Failure("User has no password set.", ErrorCode.Conflict);
            }
            if (!_passwordHasher.VerifyHashedPassword(command.Password, user.Password))
            {
                _logger.LogWarning("[Auth Service(LoginCommandHandler)] Invalid password for user with credential: {Credential}.", command.Credential);
                return TResult<AuthResult>.Failure("Invalid password.", ErrorCode.Validation);
            }

            var result = await _authFacade.BuildAuthResult(user, command.DeviceId, cancellationToken: cancellationToken);

            _logger.LogInformation("[Auth Service(LoginCommandHandler)] User with Id: {UserId} logged in successfully.", user.Id);
            return result;
        }
    }
}
