using FluentValidation;

namespace Auth.Core.Application.Auth.Commands.VerifyResetCode
{
    public class VerifyResetCodeCommandValidator : AbstractValidator<VerifyResetCodeCommand>
    {
        public VerifyResetCodeCommandValidator()
        {
            RuleFor(x => x.SessionToken)
                .NotEmpty().WithMessage("Session token is required.");

            RuleFor(x => x.VerificationCode)
                .NotEmpty().WithMessage("Verification code is required.")
                .Length(6).WithMessage("Verification code must be exactly 6 characters long.");
        }
    }
}
