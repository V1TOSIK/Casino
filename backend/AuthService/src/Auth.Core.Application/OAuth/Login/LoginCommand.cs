using Auth.Core.Application.Models;
using SharedKernel.Domain.Results;
using MediatR;

namespace Auth.Core.Application.OAuth.Login
{
    public class LoginCommand : IRequest<TResult<AuthResult>>
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public string? Email { get; set; }
        public Guid DeviceId { get; set; }

        public LoginCommand(string provider,
            string providerUserId,
            string? email,
            Guid deviceId)
        {
            Provider = provider;
            ProviderUserId = providerUserId;
            Email = email;
            DeviceId = deviceId;
        }
    }
}
