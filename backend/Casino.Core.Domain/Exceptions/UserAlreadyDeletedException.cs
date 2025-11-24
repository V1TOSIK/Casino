using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class UserAlreadyDeletedException : BaseException
    {   
        private const string DefaultErrorCode = "USER_ALREADY_DELETED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public UserAlreadyDeletedException(string message = "User already deleted.")
            : base(message, DefaultErrorCode) { }
        public UserAlreadyDeletedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public UserAlreadyDeletedException(Exception innerException)
            : base("User already deleted.", innerException, DefaultErrorCode) { }
    }
}
