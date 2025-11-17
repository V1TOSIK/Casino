using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.DbInitializer
{
    public class DbInitializer<TContext> : IDbInitializer
        where TContext : DbContext
    {
        public async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            await context.Database.MigrateAsync();

            if (context is ISeedableDbContext seedableContext)
            {
                await seedableContext.SeedAsync();
            }
        }
    }
}
