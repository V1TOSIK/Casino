using Auth.Core.Application.Models;
using SharedKernel.Domain.Results;
using MediatR;

namespace Auth.Core.Application.Auth.Commands.VerifyResetCode
{
    public class VerifyResetCodeCommand : IRequest<TResult<VerifyResetCodeResult>>
    {
        public string SessionToken { get; }
        public string VerificationCode { get; }
        public VerifyResetCodeCommand(string sessionToken, string verificationCode)
        {
            SessionToken = sessionToken;
            VerificationCode = verificationCode;
        }
    }
}
