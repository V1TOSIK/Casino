using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class UserNotDeletedException : BaseException
    {
        private const string DefaultErrorCode = "USER_NOT_DELETED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public UserNotDeletedException(string message = "User is not deleted.")
            : base(message, DefaultErrorCode) { }
        public UserNotDeletedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public UserNotDeletedException(Exception innerException)
            : base("User is not deleted.", innerException, DefaultErrorCode) { }
    }
}

