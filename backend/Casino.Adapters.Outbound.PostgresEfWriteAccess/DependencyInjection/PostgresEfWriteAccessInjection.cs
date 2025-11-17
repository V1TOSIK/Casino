using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Casino.Adapters.Outbound.PostgresEfWriteAccess.DependencyInjection
{
    public static class PostgresEfWriteAccessInjection
    {
        public static IServiceCollection AddPostgresEfWriteAccessInjection(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<PostgresEfWriteAccessDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                }));

            return services;
        }
    }
}
