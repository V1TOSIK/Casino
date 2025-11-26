using MediatR;
using SharedKernel.Domain.Results;

namespace User.Core.Application.User.Commands.Ban
{
    public class BanCommand : IRequest<Result>
    {
        public BanCommand(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; }
    }
}
