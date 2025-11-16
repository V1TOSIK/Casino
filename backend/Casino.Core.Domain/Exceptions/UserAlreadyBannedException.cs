using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class UserAlreadyBannedException : BaseException
    {
        private const string DefaultErrorCode = "USER_ALREADY_BANNED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public UserAlreadyBannedException(string message = "User already banned.")
            : base(message, DefaultErrorCode) { }
        public UserAlreadyBannedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public UserAlreadyBannedException(Exception innerException)
            : base("User already banned.", innerException, DefaultErrorCode) { }
    }
}
