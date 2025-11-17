using Auth.Adapters.Outbound.Common.Options;
using Auth.Adapters.Outbound.Common.Services;
using Auth.Core.Application.Ports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Adapters.Outbound.Common.DependencyInjection
{
    public static class AuthCommonInjection
    {
        public static IServiceCollection AddCommonBoundInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JWT"));

            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IVerificationCodeService, VerificationCodeService>();

            return services;
        }
    }
}
