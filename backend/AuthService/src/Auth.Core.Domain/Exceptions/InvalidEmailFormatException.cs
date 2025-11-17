using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class InvalidEmailFormatException : BaseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        private const string DefaultErrorCode = "INVALID_EMAIL_FORMAT";

        public InvalidEmailFormatException(string message = "Email is not in a valid format.")
            : base(message, DefaultErrorCode) { }

        public InvalidEmailFormatException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }

        public InvalidEmailFormatException(Exception innerException)
            : base("Email is not in a valid format.", innerException, DefaultErrorCode) { }
    }
}
