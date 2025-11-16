using SharedKernel.Exceptions;
using System.Net;

namespace Casino.Core.Domain.Exceptions
{
    public class InvalidPermissionNameException : BaseException
    {
        private const string DefaultErrorCode = "INVALID_PERMISSION";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public InvalidPermissionNameException(string message = "Invalid permission name.")
            : base(message, DefaultErrorCode) { }
        public InvalidPermissionNameException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public InvalidPermissionNameException(Exception innerException)
            : base("Invalid permission name.", innerException, DefaultErrorCode) { }
    }
}
