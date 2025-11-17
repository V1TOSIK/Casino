using Auth.Core.Application.Models;
using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<TResult<ForgotPasswordResult>>
    {
        public string Credential { get; }

        public ForgotPasswordCommand(string credential)
        {
            Credential = credential;
        }
    }
}
