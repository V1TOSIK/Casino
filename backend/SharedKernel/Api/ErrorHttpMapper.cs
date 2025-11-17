using Microsoft.AspNetCore.Http;
using SharedKernel.Enums;

namespace SharedKernel.Api
{
    public static class ErrorHttpMapper
    {
        public static int MapToStatusCode(ErrorCode code) =>
            code switch
            {
                ErrorCode.Validation => StatusCodes.Status400BadRequest,
                ErrorCode.NotFound => StatusCodes.Status404NotFound,
                ErrorCode.Forbidden => StatusCodes.Status403Forbidden,
                ErrorCode.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
    }
}
