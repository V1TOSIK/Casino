using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class InvalidRoleNameException : BaseException
    {
        private const string DefaultErrorCode = "INVALID_ROLE_NAME";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public InvalidRoleNameException(string message = "Invalid role name.")
            : base(message, DefaultErrorCode) { }
        public InvalidRoleNameException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public InvalidRoleNameException(Exception innerException)
            : base("Invalid role name.", innerException, DefaultErrorCode) { }
    }
}
