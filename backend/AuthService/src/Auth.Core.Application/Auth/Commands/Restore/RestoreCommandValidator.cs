using FluentValidation;

namespace Auth.Core.Application.Auth.Commands.Restore
{
    public class RestoreCommandValidator : AbstractValidator<RestoreCommand>
    {
        public RestoreCommandValidator()
        {
            RuleFor(x => x.Credential)
                .NotEmpty().WithMessage("Credential is required.")
                .MaximumLength(256).WithMessage("Credential must not exceed 256 characters.");

            RuleFor(x => x.DeviceId)
                .NotNull().WithMessage("DeviceId cannot be empty.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
        }
    }
}
