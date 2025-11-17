using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class EmptyRoleIdException : BaseException
    {
        private const string DefaultErrorCode = "EMPTY_ROLE_ID";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public EmptyRoleIdException(string message = "RoleId cannot be empty.")
            : base(message, DefaultErrorCode) { }
        public EmptyRoleIdException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public EmptyRoleIdException(Exception innerException)
            : base("RoleId cannot be empty.", innerException, DefaultErrorCode) { }
    }
}
