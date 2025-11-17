using Auth.Adapters.Inbound.Api.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Adapters.Inbound.Api.DependencyInjection
{
    public static class ApiInjection
    {
        public static IServiceCollection AddApiInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleOptions>(configuration.GetSection("GOOGLE"));
            return services;
        }
    }
}
