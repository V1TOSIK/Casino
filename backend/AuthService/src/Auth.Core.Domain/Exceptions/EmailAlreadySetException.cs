using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class EmailAlreadySetException : BaseException
    {
        private const string DefaultErrorCode = "EMAIL_ALREADY_SET";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public EmailAlreadySetException(string message = "Email has already been set and cannot be changed.")
            : base(message, DefaultErrorCode) { }
        public EmailAlreadySetException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public EmailAlreadySetException(Exception innerException)
            : base("Email has already been set and cannot be changed.", innerException, DefaultErrorCode) { }
    }
}
