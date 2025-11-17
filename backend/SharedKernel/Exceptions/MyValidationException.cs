using System.Net;

namespace SharedKernel.Exceptions
{
    public class MyValidationException : BaseException
    {
        private const string DefaultErrorCode = "INVALID_VALIDATION";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public MyValidationException(string message = "Invalid validation result.")
            : base(message, DefaultErrorCode) { }
        public MyValidationException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public MyValidationException(Exception innerException)
            : base("Invalid validation result.", innerException, DefaultErrorCode) { }
    }
}
