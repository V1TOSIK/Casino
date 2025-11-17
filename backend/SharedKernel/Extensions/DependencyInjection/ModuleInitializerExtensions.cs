using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.DbInitializer;

namespace SharedKernel.Extensions.DependencyInjection
{
    public static class ModuleInitializerExtensions
    {
        public static IServiceCollection AddModuleInitializer<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IDbInitializer, DbInitializer<TContext>>();
            return services;
        }
    }
}
