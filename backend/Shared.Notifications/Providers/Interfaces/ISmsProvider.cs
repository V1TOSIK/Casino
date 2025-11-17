namespace Shared.Notifications.Providers.Interfaces
{
    public interface ISmsProvider
    {
        Task SendAsync(string to, string message, CancellationToken cancellationToken);
    }
}
