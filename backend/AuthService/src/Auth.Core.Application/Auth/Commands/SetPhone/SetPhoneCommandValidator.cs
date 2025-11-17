using FluentValidation;

namespace Auth.Core.Application.Auth.Commands.SetPhone
{
    public class SetPhoneCommandValidator : AbstractValidator<SetPhoneCommand>
    {
        public SetPhoneCommandValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number is not valid.");
        }
    }
}
