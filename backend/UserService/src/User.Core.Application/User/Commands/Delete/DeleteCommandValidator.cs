using FluentValidation;

namespace User.Core.Application.User.Commands.Delete
{
    public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
    {
        public DeleteCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id cannot be empty");
        }
    }
}
