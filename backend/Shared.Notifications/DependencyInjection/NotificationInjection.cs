using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Notifications.Options;
using Shared.Notifications.Providers.EmailProvider;
using Shared.Notifications.Providers.Interfaces;
using Shared.Notifications.Providers.SmsProvider;

namespace Shared.Notifications.DependencyInjection
{
    public static class NotificationInjection
    {
        public static IServiceCollection AddNotificationInjection(
           this IServiceCollection services,
           IConfiguration configuration)
        {

            services.Configure<EmailOptions>(configuration.GetSection("SMTP"));
            services.AddTransient<IEmailProvider, MailKitEmailProvider>();

            services.Configure<SmsOptions>(configuration.GetSection("TWILIO"));
            services.AddTransient<ISmsProvider, TwilioSmsProvider>();
            return services;
        }
    }
}
