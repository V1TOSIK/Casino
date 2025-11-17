using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class NullablePhoneNumberException : BaseException
    {
        private const string DefaultErrorCode = "NULLABLE_PHONE_NUMBER";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public NullablePhoneNumberException(string message = "Phone number cannot be null or empty.")
            : base(message, DefaultErrorCode) { }
        public NullablePhoneNumberException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public NullablePhoneNumberException(Exception innerException)
            : base("Phone number cannot be null or empty.", innerException, DefaultErrorCode) { }
    }
}
