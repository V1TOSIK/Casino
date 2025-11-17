using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class RoleNotFoundException : BaseException
    {
        private const string DefaultErrorCode = "ROLE_NOT_FOUND";
        public override HttpStatusCode StatusCode => HttpStatusCode.NoContent;

        public RoleNotFoundException(string message = "Role not found.")
            : base(message, DefaultErrorCode) { }
        public RoleNotFoundException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public RoleNotFoundException(Exception innerException)
            : base("Role not found.", innerException, DefaultErrorCode) { }
    }
}
