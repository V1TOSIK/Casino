using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain.Errors;
using SharedKernel.Domain.Results;

namespace SharedKernel.Api
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult(this Result result, Func<IActionResult>? onSuccess = null)
        {
            if (result.IsSuccess)
                return onSuccess != null ? onSuccess() : new OkResult();

            return ErrorResponse(result.Error);
        }

        public static IActionResult ToActionResult<T>(this TResult<T> result, Func<T, IActionResult>? onSuccess = null)
        {
            if (result.IsSuccess)
                return onSuccess != null ? onSuccess(result.Value!) : new OkObjectResult(result.Value);

            return ErrorResponse(result.Error);
        }

        private static IActionResult ErrorResponse(Error? error)
        {
            var statusCode = ErrorHttpMapper.MapToStatusCode(error!.Code);
            return new ObjectResult(new ErrorResponse(error.Message, statusCode)) { StatusCode = statusCode };
        }
    }
}
