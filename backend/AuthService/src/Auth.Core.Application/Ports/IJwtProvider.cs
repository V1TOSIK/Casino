namespace Auth.Core.Application.Ports
{
    public interface IJwtProvider
    {
        Task<string> GenerateAccessToken(Guid userId, string role, CancellationToken cancellationToken);
    }
}
