using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class EmptyUserIdException : BaseException
    {
        private const string DefaultErrorCode = "EMPTY_USER_ID";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public EmptyUserIdException(string message = "User ID cannot be null or empty.")
            : base(message, DefaultErrorCode) { }
        public EmptyUserIdException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public EmptyUserIdException(Exception innerException)
            : base("User ID cannot be null or empty.", innerException, DefaultErrorCode) { }
    }
}
