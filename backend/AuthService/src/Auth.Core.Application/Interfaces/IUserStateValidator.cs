using Auth.Core.Application.Models;
using Auth.Core.Domain.Entities;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Interfaces
{
    public interface IUserStateValidator
    {
        TResult<AuthResult> Validate(UserEntity user);
    }
}
