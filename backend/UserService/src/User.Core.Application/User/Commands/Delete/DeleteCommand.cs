using MediatR;
using SharedKernel.Domain.Results;

namespace User.Core.Application.User.Commands.Delete
{
    public class DeleteCommand : IRequest<Result>
    {
        public DeleteCommand(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; set; }
    }
}
