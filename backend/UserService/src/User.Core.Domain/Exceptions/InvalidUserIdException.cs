using System.Net;
using SharedKernel.Exceptions;

namespace User.Core.Domain.Exceptions
{
    public class InvalidUserIdException : BaseException
    {
        private const string DefaultErrorCode = "INVALID_USER_ID";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public InvalidUserIdException(string message = "Invalid user Id.")
            : base(message, DefaultErrorCode) { }
        public InvalidUserIdException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public InvalidUserIdException(Exception innerException)
            : base("Invalid user Id.", innerException, DefaultErrorCode) { }
    }
}
