using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.SetEmail
{
    public class SetEmailCommand : IRequest<Result>
    {
        public string Email { get; set; } = string.Empty;
        public SetEmailCommand(string email)
        {
            Email = email;
        }
    }
}
