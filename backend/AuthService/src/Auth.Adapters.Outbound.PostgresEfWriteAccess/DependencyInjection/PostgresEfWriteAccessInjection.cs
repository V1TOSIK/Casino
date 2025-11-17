using Auth.Adapters.Outbound.PostgresEfWriteAccess.Repositories;
using Auth.Adapters.Outbound.PostgresEfWriteAccess.UnitOfWork;
using Auth.Core.Application.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.DbInitializer;
using SharedKernel.Extensions.DependencyInjection;
using SharedKernel.UnitOfWork;

namespace Auth.Adapters.Outbound.PostgresEfWriteAccess.DependencyInjection
{
    public static class PostgresEfWriteAccessInjection
    {
        public static IServiceCollection AddPostgresEfWriteAccessInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PostgresEfWriteAccessDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("AuthPostgresWriteDb"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                }));

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddModuleInitializer<PostgresEfWriteAccessDbContext>();
            services.AddScoped<ISeedableDbContext, PostgresEfWriteAccessDbContext>(provider =>
                provider.GetRequiredService<PostgresEfWriteAccessDbContext>());

            services.AddScoped<IUnitOfWork<PostgresEfWriteAccessDbContext>, UnitOfWork<PostgresEfWriteAccessDbContext>>();
            services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();

            return services;
        }
    }
}
