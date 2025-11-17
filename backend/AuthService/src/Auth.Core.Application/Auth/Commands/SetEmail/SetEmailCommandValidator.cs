using FluentValidation;

namespace Auth.Core.Application.Auth.Commands.SetEmail
{
    public class SetEmailCommandValidator : AbstractValidator<SetEmailCommand>
    {
        public SetEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("New email is required.")
                .EmailAddress().WithMessage("New email must be a valid email address.")
                .MaximumLength(256).WithMessage("New email must not exceed 256 characters.");
        }
    }
}
