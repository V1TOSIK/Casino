using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class InvalidProviderException : BaseException
    {
        private const string DefaultErrorCode = "INVALID_PROVIDER_NAME";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public InvalidProviderException(string message = "Provider name is invalid.")
            : base(message, DefaultErrorCode) { }
        public InvalidProviderException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public InvalidProviderException(Exception innerException)
            : base("Provider name is invalid.", innerException, DefaultErrorCode) { }
    }
}
