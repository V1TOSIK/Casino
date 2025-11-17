using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class UserRegisteredByProviderException : BaseException
    {
        private const string DefaultErrorCode = "USER_REGISTERED_BY_PROVIDER";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public UserRegisteredByProviderException(string message = "User is registered by an external provider.")
            : base(message, DefaultErrorCode) { }
        public UserRegisteredByProviderException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public UserRegisteredByProviderException(Exception innerException)
            : base("User is registered by an external provider.", innerException, DefaultErrorCode) { }
    }
}
