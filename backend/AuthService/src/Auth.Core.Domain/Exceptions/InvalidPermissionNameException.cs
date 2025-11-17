using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class InvalidPermissionNameException : BaseException
    {
        private const string DefaultErrorCode = "INVALID_PERMISSION_NAME";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public InvalidPermissionNameException(string message = "Permission name is invalid.")
            : base(message, DefaultErrorCode) { }
        public InvalidPermissionNameException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public InvalidPermissionNameException(Exception innerException)
            : base("Permission name is invalid.", innerException, DefaultErrorCode) { }
    }
}
