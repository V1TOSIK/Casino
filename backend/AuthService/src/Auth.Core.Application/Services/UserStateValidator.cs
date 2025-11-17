using Auth.Core.Application.Interfaces;
using Auth.Core.Application.Models;
using Auth.Core.Domain.Entities;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;

namespace Auth.Core.Application.Services
{
    public class UserStateValidator : IUserStateValidator
    {
        public TResult<AuthResult> Validate(UserEntity user)
        {
            if (user.IsDeleted)
            {
                var deletionPeriod = TimeSpan.FromDays(7);
                var timeSinceDeletion = DateTime.UtcNow - user.DeletedAt;
                var timeLeft = deletionPeriod - timeSinceDeletion;

                if (!timeLeft.HasValue)
                    timeLeft = deletionPeriod;

                if (timeLeft < TimeSpan.Zero)
                    timeLeft = TimeSpan.Zero;

                var message = $"Ваш акаунт знаходиться на видаленні. До повного видалення акаунту залишилося днів: {timeLeft.Value.Days}";

                return TResult<AuthResult>.Failure(message, ErrorCode.AccountDeleted);
            }
            if (user.IsBanned)
            {
                var message = $"Ваш акаунт заблоковано по причині: {user.BanReason}.";

                return TResult<AuthResult>.Failure(message, ErrorCode.Forbidden);

            }
            return TResult<AuthResult>.Success(default!);
        }
    }
}
