using System.Net;

namespace SharedKernel.Exceptions
{
    public abstract class BaseException : Exception
    {
        public abstract HttpStatusCode StatusCode { get; }
        public string? ErrorCode { get; }

        protected BaseException(string message, string? errorCode = null)
            : base(message) => ErrorCode = errorCode;

        protected BaseException(string message, Exception innerException, string? errorCode = null)
            : base(message, innerException) => ErrorCode = errorCode;
    }
}
