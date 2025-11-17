using SharedKernel.Domain.Errors;
using SharedKernel.Enums;

namespace SharedKernel.Domain.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string errorMessage, ErrorCode code) => new Result(false, new Error(errorMessage, code));
    }
}
