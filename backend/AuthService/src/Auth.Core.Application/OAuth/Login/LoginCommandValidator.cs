using FluentValidation;

namespace Auth.Core.Application.OAuth.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.ProviderUserId)
                .NotEmpty().WithMessage("ProviderUserId is required.");
            RuleFor(x => x.Provider)
                .NotEmpty().WithMessage("Provider is required.");
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email)).WithMessage("Email is not valid.");
            RuleFor(x => x.DeviceId)
                .NotNull().WithMessage("DeviceId cannot be null.");
        }
    }
}
