using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class NullableDeviceException : BaseException
    {
        private const string DefaultErrorCode = "NULLABLE_DEVICE";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public NullableDeviceException(string message = "Device cannot be null or empty.")
            : base(message, DefaultErrorCode) { }
        public NullableDeviceException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public NullableDeviceException(Exception innerException)
            : base("Device cannot be null or empty.", innerException, DefaultErrorCode) { }
    }
}
