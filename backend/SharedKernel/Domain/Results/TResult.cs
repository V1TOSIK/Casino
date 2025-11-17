using SharedKernel.Domain.Errors;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace SharedKernel.Domain.Results
{
    public class TResult<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T Value { get; }
        public Error? Error { get; }

        protected TResult(bool isSuccess, T value, Error? error)
        {
            if (isSuccess && error != null)
                throw new SuccessResultWithErrorMessageException("A successful result cannot have an error message.");

            if (!isSuccess && value != null && !IsNullableType(typeof(T)))
                throw new FailureResultWithValueException("A failed result cannot have a value.");

            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static TResult<T> Success(T value) => new TResult<T>(true, value, null);

        public static TResult<T> Failure(string errorMessage, ErrorCode code) => new TResult<T>(false, default!, new Error(errorMessage, code));

        private static bool IsNullableType(Type type) =>
            !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }
}
