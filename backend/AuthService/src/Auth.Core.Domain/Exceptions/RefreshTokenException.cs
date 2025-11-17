using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class RefreshTokenException : BaseException
    {
        private const string DefaultErrorCode = "REFRESH_TOKEN_ERROR";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public RefreshTokenException(string message = "Refresh token exception.")
            : base(message, DefaultErrorCode) { }
        public RefreshTokenException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public RefreshTokenException(Exception innerException)
            : base("Refresh token exception.", innerException, DefaultErrorCode) { }
    }
}
