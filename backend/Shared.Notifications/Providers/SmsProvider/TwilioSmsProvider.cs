using Microsoft.Extensions.Options;
using Shared.Notifications.Options;
using Shared.Notifications.Providers.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Shared.Notifications.Providers.SmsProvider
{
    public class TwilioSmsProvider : ISmsProvider
    {
        private readonly SmsOptions _options;
        public TwilioSmsProvider(IOptions<SmsOptions> options)
        {
            _options = options.Value;
            TwilioClient.Init(_options.AccountSid, _options.AuthToken);
        }
        public async Task SendAsync(string to, string message, CancellationToken cancellationToken)
        {
            await MessageResource.CreateAsync(
            to: new PhoneNumber(to),
            from: new PhoneNumber(_options.FromPhone),
            body: message
        );
        }
    }
}
