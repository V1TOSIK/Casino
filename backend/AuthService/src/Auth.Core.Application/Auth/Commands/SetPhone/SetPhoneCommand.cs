using MediatR;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Auth.Commands.SetPhone
{
    public class SetPhoneCommand : IRequest<Result>
    {
        public string PhoneNumber { get; }
        public SetPhoneCommand(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
