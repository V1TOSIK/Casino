using Auth.Core.Application.Models;
using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.Login
{
    public class LoginCommand : IRequest<TResult<AuthResult>>
    {
        public Guid DeviceId { get; set; }
        public string Credential { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginCommand(Guid deviceId,
            string credential,
            string password)
        {
            DeviceId = deviceId;
            Credential = credential;
            Password = password;
        }
    }
}
