using FluentValidation;

namespace Auth.Core.Application.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.")
                .MaximumLength(100).WithMessage("Current password must not exceed 100 characters.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("New password must not exceed 100 characters.");
        }
    }
}
