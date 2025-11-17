using FluentValidation;
using SharedKernel.Domain.Regexs;

namespace Auth.Core.Application.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.DeviceId)
                .NotNull().WithMessage("DeviceId cannot be null.");

            RuleFor(x => x.Credential)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Credential is required.")
                .MaximumLength(100).WithMessage("Credential must not exceed 100 characters.")
                .Must(x => x.IsValidEmailOrPhone())
                    .WithErrorCode("InvalidCredential")
                    .WithMessage("Credential must be a valid email or phone number.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
        }
    }
}
