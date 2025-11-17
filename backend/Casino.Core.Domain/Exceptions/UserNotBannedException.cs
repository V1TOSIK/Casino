using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class UserNotBannedException : BaseException
    {
        private const string DefaultErrorCode = "USER_NOT_BANNED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public UserNotBannedException(string message = "User is not banned.")
            : base(message, DefaultErrorCode) { }
        public UserNotBannedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public UserNotBannedException(Exception innerException)
            : base("User is not banned.", innerException, DefaultErrorCode) { }
    }
}
