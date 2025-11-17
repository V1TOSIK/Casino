using Auth.Core.Application.Models;
using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<TResult<AuthResult>>
    {
        public Guid DeviceId { get; }
        public string Credential { get; }
        public string Password { get; }
        public RegisterCommand(Guid deviceId, string credential, string password)
        {
            DeviceId = deviceId;
            Credential = credential;
            Password = password;
        }
    }
}
