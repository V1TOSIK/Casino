using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class InvalidCredentialTypeException : BaseException
    {
        private const string DefaultErrorCode = "INVALID_CREDENTIAL_TYPE";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public InvalidCredentialTypeException(string message = "Invalid credential type.")
            : base(message, DefaultErrorCode) { }
        public InvalidCredentialTypeException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public InvalidCredentialTypeException(Exception innerException)
            : base("Invalid credential type.", innerException, DefaultErrorCode) { }
    }
}
