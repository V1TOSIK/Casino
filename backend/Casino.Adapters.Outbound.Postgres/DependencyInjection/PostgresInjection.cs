using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Casino.Adapters.Outbound.Postgres.DependencyInjection
{
    public static class PostgresInjection
    {
        public static IServiceCollection AddPostgresAdapter(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<CasinoDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("CasinoPostgres"),
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
