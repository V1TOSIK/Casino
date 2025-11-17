using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class NullablePasswordException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        private const string DefaultErrorCode = "NULLABLE_PASSWORD";

        public NullablePasswordException(string message = "Password cannot be null or empty.")
            : base(message, DefaultErrorCode) { }
        public NullablePasswordException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public NullablePasswordException(Exception innerException)
            : base("Password cannot be null or empty.", innerException, DefaultErrorCode) { }
    }
}
