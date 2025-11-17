using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Auth.Adapters.Outbound.PostgresEfWriteAccess
{
    public class PostgresEfWriteAccessDbContextFactory : IDesignTimeDbContextFactory<PostgresEfWriteAccessDbContext>
    {
        public PostgresEfWriteAccessDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PostgresEfWriteAccessDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("AuthPostgresWriteDb"));

            return new PostgresEfWriteAccessDbContext(optionsBuilder.Options);
        }
    }
}
