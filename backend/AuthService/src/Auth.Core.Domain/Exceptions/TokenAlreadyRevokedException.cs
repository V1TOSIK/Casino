using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class TokenAlreadyRevokedException : BaseException
    {
        private const string DefaultErrorCode = "TOKEN_ALREADY_REVOKED";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public TokenAlreadyRevokedException(string message = "Token already revoked.")
            : base(message, DefaultErrorCode) { }
        public TokenAlreadyRevokedException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public TokenAlreadyRevokedException(Exception innerException)
            : base("Token already revoked.", innerException, DefaultErrorCode) { }
    }
}
