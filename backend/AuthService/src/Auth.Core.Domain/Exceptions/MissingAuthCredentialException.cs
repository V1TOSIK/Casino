using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class MissingAuthCredentialException : BaseException
    {
        private const string DefaultErrorCode = "MISSING_AUTH_CREDENTIAL";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public MissingAuthCredentialException(string message = "At least one authentication credential (email or phone number) must be provided.")
            : base(message, DefaultErrorCode) { }
        public MissingAuthCredentialException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public MissingAuthCredentialException(Exception innerException)
            : base("At least one authentication credential (email or phone number) must be provided.", innerException, DefaultErrorCode) { }
    }
}
