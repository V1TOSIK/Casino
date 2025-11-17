using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class ExternalLoginAlreadyExistException : BaseException
    {
        private const string DefaultErrorCode = "EXTERNAL_LOGIN_ALREADY_EXIST";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public ExternalLoginAlreadyExistException(string message = "The external login already exists.")
            : base(message, DefaultErrorCode) { }
        public ExternalLoginAlreadyExistException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public ExternalLoginAlreadyExistException(Exception innerException)
            : base("The external login already exists.", innerException, DefaultErrorCode) { }
    }
}
