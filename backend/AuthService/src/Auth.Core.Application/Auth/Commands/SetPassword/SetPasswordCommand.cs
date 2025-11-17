using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.SetPassword
{
    public class SetPasswordCommand : IRequest<Result>
    {
        public string Password { get; }
        public SetPasswordCommand(string password)
        {
            Password = password;
        }
    }
}
