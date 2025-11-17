using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Ports
{
    public interface IVerificationCodeService
    {
        Task<Result> SendVerificationCodeAsync(string destination, string verificationCode, CancellationToken cancellationToken);
        string GenerateVerificationCode();
    }
}
