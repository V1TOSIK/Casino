using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Extensions.DependencyInjection;
using SharedKernel.UnitOfWork;
using User.Adapters.Outbound.Postgres.Repositories;
using User.Adapters.Outbound.Postgres.UnitOfWork;
using User.Core.Application.Ports;

namespace User.Adapters.Outbound.Postgres.DependencyInjection
{
    public static class PostgresInjection
    {
        public static IServiceCollection AddPostgresInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("UserPostgres"),
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                }));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddModuleInitializer<UserDbContext>();

            services.AddScoped<IUnitOfWork<UserDbContext>, UnitOfWork<UserDbContext>>();
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
            return services;
        }
    }
}
