using System.Net;

namespace SharedKernel.Exceptions
{
    public class SuccessResultWithErrorMessageException : BaseException
    {
        private const string DefaultErrorCode = "SUCCESS_RESULT_WITH_ERROR_MESSAGE";
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public SuccessResultWithErrorMessageException(string message = "Success result can not have error message.")
            : base(message, DefaultErrorCode) { }
        public SuccessResultWithErrorMessageException(string message, Exception innerException)
            : base(message, innerException, DefaultErrorCode) { }
        public SuccessResultWithErrorMessageException(Exception innerException)
            : base("Success result can not have error message.", innerException, DefaultErrorCode) { }
    }
}
