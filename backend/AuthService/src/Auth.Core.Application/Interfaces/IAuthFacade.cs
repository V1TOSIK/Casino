using Auth.Core.Application.Models;
using Auth.Core.Domain.Entities;
using SharedKernel.Domain.Results;

namespace Auth.Core.Application.Interfaces
{
    public interface IAuthFacade
    {
        Task<TResult<AuthResult>> BuildAuthResult(UserEntity user,
            Guid deviceId,
            Guid? tokenId = null,
            CancellationToken cancellationToken = default);
    }
}