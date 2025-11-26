using FluentValidation;

namespace User.Core.Application.User.Commands.Ban
{
    public class BanCommandValidator : AbstractValidator<BanCommand>
    {
        public BanCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id cannot be empty");
        }
    }
}
