using FluentValidation;

namespace Auth.Core.Application.Auth.Commands.LogoutAll
{
    public class LogoutAllValidator : AbstractValidator<LogoutAllCommand>
    {
        public LogoutAllValidator()
        {
            // No specific validation rules as UserId is optional
        }
    }
}
