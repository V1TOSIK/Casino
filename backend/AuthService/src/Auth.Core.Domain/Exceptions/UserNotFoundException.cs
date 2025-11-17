using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class UserNotFoundException : BaseException
    {
        private const string DefaultErrorCode = "USER_NOT_FOUND";
        public override HttpStatusCode StatusCode => HttpStatusCode.NoContent;

        public UserNotFoundException(string message = "user not found")
            : base(message, DefaultErrorCode) { }
        public UserNotFoundException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public UserNotFoundException(Exception innerException)
            : base("RoleId cannot be empty.", innerException, DefaultErrorCode) { }
    }
}
