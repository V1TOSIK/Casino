using Auth.Core.Application.Models;
using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.Refresh
{
    public class RefreshCommand : IRequest<TResult<AuthResult>>
    {
        public Guid DeviceId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;

        public RefreshCommand(Guid deviceId, string refreshToken)
        {
            DeviceId = deviceId;
            RefreshToken = refreshToken;
        }
    }
}
