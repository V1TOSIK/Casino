using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class NullableIpAddressException : BaseException
    {
        private const string DefaultErrorCode = "NULLABLE_IP_ADDRESS";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public NullableIpAddressException(string message = "IP address cannot be null or empty.")
            : base(message, DefaultErrorCode) { }
        public NullableIpAddressException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public NullableIpAddressException(Exception innerException)
            : base("IP address cannot be null or empty.", innerException, DefaultErrorCode) { }
    }
}
