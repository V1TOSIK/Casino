using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class NullableEmailException : BaseException
    {
        private const string DefaultErrorCode = "NULLABLE_EMAIL";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public NullableEmailException(string message = "Email cannot be null or empty.")
            : base(message, DefaultErrorCode) { }
        public NullableEmailException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public NullableEmailException(Exception innerException)
            : base("Email cannot be null or empty.", innerException, DefaultErrorCode) { }
    }
}
