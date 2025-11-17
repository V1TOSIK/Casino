using FluentValidation;
using SharedKernel.Domain.Regexs;

namespace Auth.Core.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Credential)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Credential is required.")
                    .MaximumLength(100).WithMessage("Credential must not exceed 100 characters.")
                    .Must(x => x.IsValidEmailOrPhone())
                        .WithErrorCode("InvalidCredential")
                        .WithMessage("Credential must be a valid email or phone number.");
        }
    }
}
