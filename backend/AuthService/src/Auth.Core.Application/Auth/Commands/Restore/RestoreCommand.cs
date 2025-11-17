using Auth.Core.Application.Models;
using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.Restore
{
    public class RestoreCommand : IRequest<TResult<AuthResult>>
    {
        public string Credential { get; }
        public Guid DeviceId { get; }
        public string Password { get; }

        public RestoreCommand(string credential,
            Guid deviceId,
            string password)
        {
            Credential = credential;
            DeviceId = deviceId;
            Password = password;
        }
    }
}
