using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.LogoutAll
{
    public class LogoutAllCommand : IRequest<Result>
    {
        public Guid? UserId { get; } = null;

        public LogoutAllCommand()
        {
            UserId = null;
        }

        public LogoutAllCommand(Guid? userId)
        {
            UserId = userId;
        }
    }
}
