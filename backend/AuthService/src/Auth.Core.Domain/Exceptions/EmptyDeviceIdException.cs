using SharedKernel.Exceptions;
using System.Net;

namespace Auth.Core.Domain.Exceptions
{
    public class EmptyDeviceIdException : BaseException
    {
        private const string DefaultErrorCode = "EMPTY_DEVICE_ID";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public EmptyDeviceIdException(string message = "Device ID cannot be null or empty.")
            : base(message, DefaultErrorCode) { }
        public EmptyDeviceIdException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public EmptyDeviceIdException(Exception innerException)
            : base("Device ID cannot be null or empty.", innerException, DefaultErrorCode) { }
    }
}
