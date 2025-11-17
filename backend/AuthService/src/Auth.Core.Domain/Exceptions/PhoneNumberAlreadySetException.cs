using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class PhoneNumberAlreadySetException : BaseException
    {
        private const string DefaultErrorCode = "PHONE_NUMBER_ALREADY_SET";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public PhoneNumberAlreadySetException(string message = "Phone number has already been set and cannot be changed.")
            : base(message, DefaultErrorCode) { }
        public PhoneNumberAlreadySetException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public PhoneNumberAlreadySetException(Exception innerException)
            : base("Phone number has already been set and cannot be changed.", innerException, DefaultErrorCode) { }
    }
}
