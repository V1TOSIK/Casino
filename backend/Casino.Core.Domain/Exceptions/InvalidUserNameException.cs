using System.Net;
using SharedKernel.Exceptions;

namespace Casino.Core.Domain.Exceptions
{
    public class InvalidUserNameException : BaseException
    {
        private const string DefaultErrorCode = "INVALID_USER_NAME";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public InvalidUserNameException(string message = "Invalid user name.")
            : base(message, DefaultErrorCode) { }
        public InvalidUserNameException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public InvalidUserNameException(Exception innerException)
            : base("Invalid user name.", innerException, DefaultErrorCode) { }
    }
}
