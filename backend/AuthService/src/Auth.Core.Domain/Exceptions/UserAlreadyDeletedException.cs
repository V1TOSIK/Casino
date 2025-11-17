using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class UserAlreadyDeletedException : BaseException
    {
        private const string DefaultErrorCode = "USER_ALREADY_DELETED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public UserAlreadyDeletedException(string message = "The user has already been deleted.")
            : base(message, DefaultErrorCode) { }
        public UserAlreadyDeletedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public UserAlreadyDeletedException(Exception innerException)
            : base("The user has already been deleted.", innerException, DefaultErrorCode) { }
    }
}
