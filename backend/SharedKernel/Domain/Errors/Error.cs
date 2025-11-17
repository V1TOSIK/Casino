using SharedKernel.Enums;

namespace SharedKernel.Domain.Errors
{
    public class Error
    {
        public string Message { get; }
        public ErrorCode Code { get; }

        public Error(string message, ErrorCode code)
        {
            Message = message;
            Code = code;
        }
    }
}
