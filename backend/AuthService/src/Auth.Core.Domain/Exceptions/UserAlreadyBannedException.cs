using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class UserAlreadyBannedException : BaseException
    {
        private const string DefaultErrorCode = "USER_ALREADY_BANNED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public UserAlreadyBannedException(string message = "The user is already banned.")
            : base(message, DefaultErrorCode) { }
        public UserAlreadyBannedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public UserAlreadyBannedException(Exception innerException)
            : base("The user is already banned.", innerException, DefaultErrorCode) { }
    }
}
