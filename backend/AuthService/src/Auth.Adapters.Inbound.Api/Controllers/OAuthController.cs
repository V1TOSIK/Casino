using Auth.Adapters.Inbound.Api.Dtos.Requests;
using Auth.Adapters.Inbound.Api.Dtos.Responses;
using Auth.Adapters.Inbound.Api.Options;
using Auth.Core.Application.OAuth.Login;
using Auth.Core.Application.Ports;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using SharedKernel.Api;

namespace Auth.Adapters.Inbound.Api.Controllers
{
    [Route("api/v1/oauth")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly ICookieService _cookieService;
        private readonly IMediator _mediator;
        private readonly GoogleOptions _googleOptions;

        public OAuthController(
            ICookieService cookieService,
            IMediator mediator,
            IOptions<GoogleOptions> googleOptions)
        {
            _cookieService = cookieService;
            _mediator = mediator;
            _googleOptions = googleOptions.Value;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleTokenRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _googleOptions.ClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            var deviceId = _cookieService.GetDeviceId();

            var result = await _mediator.Send(new LoginCommand("Google", payload.Subject, payload.Email, deviceId), cancellationToken);

            return result.ToActionResult(value =>
            {
                _cookieService.SetRefreshTokenAndDeviceCookie(value, deviceId);
                return Ok(new LoginResponse(value.AccessToken));
            });
        }
    }
}
