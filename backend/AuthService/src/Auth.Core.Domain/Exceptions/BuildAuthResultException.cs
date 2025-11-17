using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class BuildAuthResultException : BaseException
    {
        private const string DefaultErrorCode = "BUILD_AUTH_RESULT_ERROR";
        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
        public BuildAuthResultException(string message = "An error occurred while building the authentication result.")
            : base(message, DefaultErrorCode) { }
        public BuildAuthResultException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public BuildAuthResultException(Exception innerException)
            : base("An error occurred while building the authentication result.", innerException, DefaultErrorCode) { }
    }
}
