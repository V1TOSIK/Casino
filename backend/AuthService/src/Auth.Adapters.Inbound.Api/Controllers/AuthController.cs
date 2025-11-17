using Auth.Adapters.Inbound.Api.Dtos.Requests;
using Auth.Adapters.Inbound.Api.Dtos.Responses;
using Auth.Core.Application.Auth.Commands.ChangePassword;
using Auth.Core.Application.Auth.Commands.ForgotPassword;
using Auth.Core.Application.Auth.Commands.Login;
using Auth.Core.Application.Auth.Commands.Logout;
using Auth.Core.Application.Auth.Commands.LogoutAll;
using Auth.Core.Application.Auth.Commands.Refresh;
using Auth.Core.Application.Auth.Commands.Register;
using Auth.Core.Application.Auth.Commands.ResetPassword;
using Auth.Core.Application.Auth.Commands.Restore;
using Auth.Core.Application.Auth.Commands.SetEmail;
using Auth.Core.Application.Auth.Commands.SetPassword;
using Auth.Core.Application.Auth.Commands.SetPhone;
using Auth.Core.Application.Auth.Commands.VerifyResetCode;
using Auth.Core.Application.Ports;
using AuthTools.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Api;
using SharedKernel.Statics;

namespace Auth.Adapters.Inbound.Api.Controllers
{

    [Route("api/v1/users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICookieService _cookieService;
        public AuthController(IMediator mediator, ICookieService cookieService)
        {
            _mediator = mediator;
            _cookieService = cookieService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
        {
            var deviceId = _cookieService.GetDeviceId();
            var command = new LoginCommand(deviceId, request.Credential, request.Password);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(value =>
            {
                _cookieService.SetRefreshTokenAndDeviceCookie(value, deviceId);
                return Ok(new LoginResponse(value.AccessToken));
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var deviceId = _cookieService.GetDeviceId();
            var command = new RegisterCommand(deviceId, request.Credential, request.Password);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(value =>
            {
                _cookieService.SetRefreshTokenAndDeviceCookie(value, deviceId);
                return Ok(new RegisterResponse(value.AccessToken));
            });
        }

        [HttpPost("restore")]
        public async Task<IActionResult> Restore([FromRoute] Guid userId, [FromBody] RestoreRequest request, CancellationToken cancellationToken = default)
        {
            var deviceId = _cookieService.GetDeviceId();
            var command = new RestoreCommand(request.Credential, deviceId, request.Password);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(value =>
            {
                _cookieService.SetRefreshTokenAndDeviceCookie(value, deviceId);
                return Ok(new RestoreResponse(value.AccessToken));
            });
        }

        [HttpPost("password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var command = new ForgotPasswordCommand(request.Credential);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(value =>
            {
                _cookieService.SetSessionTokenCookie(value.SessionToken, DateTime.UtcNow.AddMinutes(10));
                return Ok("If the account exists, a password reset link has been sent to the registered email or phone number.");
            });
        }

        [HttpPost("password/verification")]
        public async Task<IActionResult> VerifyResetPassword([FromBody] VerifyResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var sessionToken = _cookieService.GetSessionToken();
            if (sessionToken == null)
                return BadRequest("Invalid or expired session token");

            var command = new VerifyResetCodeCommand(sessionToken, request.VerificationCode);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(value =>
            {
                _cookieService.DeleteSessionTokenCookie();
                _cookieService.SetResetTokenCookie(value.ResetToken, DateTime.UtcNow.AddMinutes(10));
                return Ok();
            });
        }

        [HttpPatch("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var resetToken = _cookieService.GetResetToken();

            if (resetToken == null)
                return BadRequest("Invalid or expired reset token");

            var command = new ResetPasswordCommand(resetToken, request.NewPassword);
            var result = await _mediator.Send(command, cancellationToken);
            _cookieService.DeleteResetTokenCookie();
            return result.ToActionResult(() => Ok("Password has been reset successfully"));
        }

        [Authorize]
        [HttpPatch("me/password/change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand(request.CurrentPassword, request.NewPassword);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(() => Ok("Password changed successfully"));
        }

        [Authorize]
        [HttpPatch("me/password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var command = new SetPasswordCommand(request.Password);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(() => Ok("Password set successfully"));
        }

        [Authorize]
        [HttpPatch("me/email")]
        public async Task<IActionResult> SetEmail([FromBody] SetEmailRequest request, CancellationToken cancellationToken = default)
        {
            var command = new SetEmailCommand(request.Email);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(() => Ok("Email added successfully"));
        }

        [Authorize]
        [HttpPatch("me/phone")]
        public async Task<IActionResult> SetPhone([FromBody] SetPhoneRequest request, CancellationToken cancellationToken = default)
        {
            var command = new SetPhoneCommand(request.Phone);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(() => Ok("Phone number added successfully"));
        }

        [HasPermission(PermissionType.Manage, PermissionType.All)]
        [HttpDelete("{userId}/sessions")]
        public async Task<IActionResult> LogoutUser([FromRoute] Guid userId, CancellationToken cancellationToken = default)
        {
            var command = new LogoutAllCommand(userId);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(() => Ok("User logged out from device successfully"));
        }

        [Authorize]
        [HttpDelete("me/logout/all")]
        public async Task<IActionResult> LogoutAll(CancellationToken cancellationToken = default)
        {
            var command = new LogoutAllCommand();
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(() =>
            {
                _cookieService.DeleteRefreshTokenCookie();
                return Ok("User logged out from all devices successfully");
            });
        }

        [Authorize]
        [HttpDelete("me/logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
        {
            var refreshToken = _cookieService.GetRefreshToken();

            if (refreshToken == null)
                return BadRequest("Invalid request");

            var command = new LogoutCommand(refreshToken);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(() =>
            {
                _cookieService.DeleteRefreshTokenCookie();
                return Ok("User logged out successfully");
            });
        }

        [HttpPost("me/token/refresh")]
        public async Task<IActionResult> RefreshTokens(CancellationToken cancellationToken = default)
        {
            var refreshToken = _cookieService.GetRefreshToken();

            if (refreshToken == null)
                return BadRequest("Invalid request");

            var deviceId = _cookieService.GetDeviceId();

            var command = new RefreshCommand(deviceId, refreshToken);
            var result = await _mediator.Send(command, cancellationToken);

            return result.ToActionResult(value =>
            {
                _cookieService.SetRefreshTokenCookieIfNotNull(value.RefreshToken, value.Expiration);
                return Ok(new RefreshResponse(value.AccessToken));
            });
        }
    }
}
