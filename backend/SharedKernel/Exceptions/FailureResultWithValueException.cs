using System.Net;

namespace SharedKernel.Exceptions
{
    public class FailureResultWithValueException : BaseException
    {
        private const string DefaultErrorCode = "FAILURE_RESULT_WITH_VALUE";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public FailureResultWithValueException(string message = "Failure result can not have value.")
            : base(message, DefaultErrorCode) { }
        public FailureResultWithValueException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public FailureResultWithValueException(Exception innerException)
            : base("Failure result can not have value.", innerException, DefaultErrorCode) { }
    }
}
