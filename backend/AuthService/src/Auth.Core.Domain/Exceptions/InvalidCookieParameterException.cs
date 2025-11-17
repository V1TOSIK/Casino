using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class InvalidCookieParameterException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        private const string DefaultErrorCode = "INVALID_COOKIE_PARAMETER";

        public InvalidCookieParameterException(string message = "Cookie key cannot be null or empty.")
            : base(message, DefaultErrorCode) { }

        public InvalidCookieParameterException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }

        public InvalidCookieParameterException(Exception innerException)
            : base("Cookie key cannot be null or empty.", innerException, DefaultErrorCode) { }
    }
}
