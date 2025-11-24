using Auth.Adapters.Outbound.Postgres.Repositories;
using Auth.Adapters.Outbound.Postgres.UnitOfWork;
using Auth.Core.Application.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.DbInitializer;
using SharedKernel.Extensions.DependencyInjection;
using SharedKernel.UnitOfWork;

namespace Auth.Adapters.Outbound.Postgres.DependencyInjection
{
    public static class PostgresInjection
    {
        public static IServiceCollection AddPostgresAdapter(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("AuthPostgres"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                }));

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddModuleInitializer<AuthDbContext>();
            services.AddScoped<ISeedableDbContext, AuthDbContext>(provider =>
                provider.GetRequiredService<AuthDbContext>());

            services.AddScoped<IUnitOfWork<AuthDbContext>, UnitOfWork<AuthDbContext>>();
            services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();

            return services;
        }
    }
}
