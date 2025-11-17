using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class PasswordAlreadySetException : BaseException
    {
        private const string DefaultErrorCode = "PASSWORD_ALREADY_SET";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public PasswordAlreadySetException(string message = "Password has already been set and cannot be changed.")
            : base(message, DefaultErrorCode) { }
        public PasswordAlreadySetException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public PasswordAlreadySetException(Exception innerException)
            : base("Password has already been set and cannot be changed.", innerException, DefaultErrorCode) { }
    }
}
