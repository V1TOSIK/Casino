namespace Shared.Notifications.Providers.Interfaces
{
    public interface IEmailProvider
    {
        Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken);
    }
}
