using Auth.Core.Application.Interfaces;
using Auth.Core.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Validations;

namespace Auth.Core.Application.DependencyInjection
{
    public static class ApplicationInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationInjection).Assembly));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IAuthFacade, AuthFacade>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IUserStateValidator, UserStateValidator>();

            return services;
        }
    }
}
