using Auth.Core.Application.Interfaces;
using Auth.Core.Application.Models;
using Auth.Core.Application.Ports;
using Auth.Core.Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedKernel.CurrentUser;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Services
{
    public class AuthFacade : IAuthFacade
    {
        private readonly IUserStateValidator _userStateValidator;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAuthRepository _authRepository;
        private readonly IAuthUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AuthFacade> _logger;

        public AuthFacade(
            IUserStateValidator userStateValidator,
            IRefreshTokenService refreshTokenService,
            IAuthRepository authRepository,
            IAuthUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            ICurrentUserService currentUserService,
            ILogger<AuthFacade> logger)
        {
            _userStateValidator = userStateValidator;
            _refreshTokenService = refreshTokenService;
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<TResult<AuthResult>> BuildAuthResult(UserEntity user, Guid deviceId, Guid? tokenId = null, CancellationToken cancellationToken = default)
        {
            var validationResult = _userStateValidator.Validate(user);
            if (validationResult.IsFailure)
            {
                _logger.LogWarning("[Auth Service(AuthFacade)] User with ID {UserId} is in invalid state: {Error}.", user.Id, validationResult.Error);
                return validationResult;
            }

            var refreshToken = await _refreshTokenService.GenerateRefreshToken(
                user.Id,
                _currentUserService.Device ?? "",
                deviceId,
                _currentUserService.IpAddress ?? "",
                tokenId,
                cancellationToken: cancellationToken);

            var role = await _authRepository.GetRoleByIdAsync(user.RoleId, cancellationToken);

            var accessToken = await _jwtProvider.GenerateAccessToken(user.Id, role.Name.ToString(), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = new AuthResult(
                accessToken,
                refreshToken);

            return TResult<AuthResult>.Success(result);
        }
    }
}
