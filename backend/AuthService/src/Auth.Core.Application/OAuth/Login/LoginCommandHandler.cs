using Auth.Core.Application.Interfaces;
using Auth.Core.Application.Models;
using Auth.Core.Application.Ports;
using Auth.Core.Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Results;
using SharedKernel.Statics;

namespace Auth.Core.Application.OAuth.Login
{
    public class LoginCommandHandler
    {
        private readonly IAuthRepository _authRepository;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly IAuthFacade _authFacade;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IAuthRepository authRepository,
            IAuthUnitOfWork unitOfWork,
            IAuthFacade authFacade,
            ILogger<LoginCommandHandler> logger)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _authFacade = authFacade;
            _logger = logger;
        }

        public async Task<TResult<AuthResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetByOAuthAsync(command.Provider, command.ProviderUserId, cancellationToken);
            if (user == null)
            {
                var email = string.IsNullOrWhiteSpace(command.Email) ? null : command.Email;
                var roleId = await _authRepository.GetRoleIdByNameAsync(RoleType.User, cancellationToken);
                user = UserEntity.Create(email, null, null, roleId);
                user.AddExternalLogin(command.ProviderUserId, command.Provider);
                await _authRepository.AddAsync(user, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var result = await _authFacade.BuildAuthResult(user, command.DeviceId, cancellationToken: cancellationToken);

            _logger.LogInformation("[Auth Service(LoginCommandHandler)] User with Id: {UserId} logged in by OAuth successfully.", user.Id);
            return result;

        }
    }
}
