using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<Result>
    {
        public string RefreshToken { get; } = string.Empty;

        public LogoutCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
