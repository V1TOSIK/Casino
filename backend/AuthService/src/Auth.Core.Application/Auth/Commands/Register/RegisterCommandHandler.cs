using Auth.Core.Application.Interfaces;
using Auth.Core.Application.Models;
using Auth.Core.Application.Ports;
using Auth.Core.Domain.Entities;
using Auth.Core.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Regexs;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;
using SharedKernel.Statics;

namespace Auth.Core.Application.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, TResult<AuthResult>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthFacade _authFacade;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterCommandHandler> _logger;
        public RegisterCommandHandler(
            IAuthRepository authRepository,
            IPasswordHasher passwordHasher,
            IAuthFacade authFacade,
            IAuthUnitOfWork unitOfWork,
            ILogger<RegisterCommandHandler> logger)
        {
            _authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _authFacade = authFacade;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TResult<AuthResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _authRepository.GetByCredentialAsync(command.Credential, cancellationToken);

            if (existingUser != null)
            {
                _logger.LogWarning("[Auth Service(RegisterCommandHandler)] User with this credential already exists.");
                return TResult<AuthResult>.Failure("User with this credential already exists.", ErrorCode.Conflict);
            }

            var hashPassword = _passwordHasher.HashPassword(command.Password);

            var roleId = await _authRepository.GetRoleIdByNameAsync(RoleType.User, cancellationToken);

            UserEntity user = null!;
            if (RegexPatterns.Email.IsMatch(command.Credential))
            {
                if (await _authRepository.IsEmailRegisteredAsync(command.Credential, cancellationToken))
                {
                    _logger.LogError("[Auth Service(RegisterCommandHandler)] User with email {email} already exists", command.Credential);
                    return TResult<AuthResult>.Failure($"User with email {command.Credential} already exists.", ErrorCode.Conflict);
                }
                user = UserEntity.Create(command.Credential, null, hashPassword, roleId);
            }
            else if (RegexPatterns.Phone.IsMatch(command.Credential))
            {
                if (await _authRepository.IsPhoneNumberRegisteredAsync(command.Credential, cancellationToken))
                {
                    _logger.LogError("[Auth Service(RegisterCommandHandler)] User with phone number {phoneNumber} already exists", command.Credential);
                    return TResult<AuthResult>.Failure($"User with phone number {command.Credential} already exists.", ErrorCode.Conflict);
                }
                user = UserEntity.Create(command.Credential, null, hashPassword, roleId);
            }
            else
            {
                _logger.LogWarning("[Auth Service(RegisterCommandHandler)] Invalid credential format: {Credential}.", command.Credential);
                return TResult<AuthResult>.Failure("Invalid credential format. Must be a valid email or phone number.", ErrorCode.Validation);
            }

            TResult<AuthResult> result = TResult<AuthResult>.Failure("error", ErrorCode.Unknown);

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _authRepository.AddAsync(user, cancellationToken);

                result = await _authFacade.BuildAuthResult(user, command.DeviceId, cancellationToken: cancellationToken);
                if (result.IsSuccess)
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                else
                    throw new BuildAuthResultException(result.Error!.Message);
            }, cancellationToken);

            _logger.LogInformation("[Auth Service(RegisterCommandHandler)] User with credential: {Credential} registered successfully.", command.Credential);
            return result;
        }
    }
}
