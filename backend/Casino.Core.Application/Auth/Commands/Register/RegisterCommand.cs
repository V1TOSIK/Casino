using Casino.Core.Application.Models;
using MediatR;

namespace Casino.Core.Application.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<AuthResult>
    {
    }
}
