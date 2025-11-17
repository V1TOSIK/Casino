using SharedKernel.Domain.Errors;

namespace SharedKernel.Api
{
    public class ErrorResponse
    {
        public string Message { get; }
        public int StatusCode { get; }

        public ErrorResponse(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
