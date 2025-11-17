using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Result>
    {
        public string ResetToken { get; }
        public string NewPassword { get; }
        public ResetPasswordCommand(string resetToken,
            string newPassword)
        {
            ResetToken = resetToken;
            NewPassword = newPassword;
        }
    }
}
