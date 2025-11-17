using Auth.Core.Application.Ports;
using Shared.Notifications.Providers.Interfaces;
using SharedKernel.Domain.Regexs;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Adapters.Outbound.Common.Services
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly IEmailProvider _emailProvider;
        private readonly ISmsProvider _smsProvider;
        public VerificationCodeService(IEmailProvider emailProvider, ISmsProvider smsProvider)
        {
            _emailProvider = emailProvider;
            _smsProvider = smsProvider;
        }

        public async Task<Result> SendVerificationCodeAsync(string destination, string verificationCode, CancellationToken cancellationToken)
        {
            if (RegexPatterns.Email.IsMatch(destination))
            {
                var subject = "Password Reset Verification Code";
                var body = $"Your password reset verification code is: {verificationCode}";

                await _emailProvider.SendAsync(destination, subject, body, cancellationToken);
            }
            else if (RegexPatterns.Phone.IsMatch(destination))
            {
                var message = $"Your password reset verification code is: {verificationCode}";
                await _smsProvider.SendAsync(destination, message, cancellationToken);
            }
            else
                return Result.Failure("Invalid credential format.", ErrorCode.Validation);

            return Result.Success();
        }

        public string GenerateVerificationCode()
        {
            var code = new Random().Next(100000, 999999).ToString();
            return code;
        }
    }
}
