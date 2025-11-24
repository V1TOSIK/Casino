using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Casino.Adapters.Outbound.Postgres
{
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<CasinoDbContext>
    {
        public CasinoDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CasinoDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("AuthPostgres"));

            return new CasinoDbContext();
        }
    }
}
