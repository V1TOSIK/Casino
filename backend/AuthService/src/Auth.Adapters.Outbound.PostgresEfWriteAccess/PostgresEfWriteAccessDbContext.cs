using Auth.Core.Application.Ports;
using Auth.Core.Domain.Entities;
using Auth.Core.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DbInitializer;
using SharedKernel.Domain.AggregateRoot;
using SharedKernel.Statics;

namespace Auth.Adapters.Outbound.PostgresEfWriteAccess
{
    public class PostgresEfWriteAccessDbContext : DbContext, ISeedableDbContext
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMediator _mediator;

        public PostgresEfWriteAccessDbContext(DbContextOptions<PostgresEfWriteAccessDbContext> options)
            : base(options)
        {
        }

        public PostgresEfWriteAccessDbContext(
            DbContextOptions<PostgresEfWriteAccessDbContext> options,
            IPasswordHasher passwordHasher,
            IMediator mediator)
            : base(options)
        {
            _passwordHasher = passwordHasher;
            _mediator = mediator;
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ExternalLogin> ExternalLogins { get; set; }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedPermissionsAsync();
            await SeedRolesAndPermissionsAsync();
            await SeedAdminAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (!await Roles.AnyAsync())
            {
                var roles = new[]
                {
                    new RoleEntity(RoleType.Admin),
                    new RoleEntity(RoleType.Moderator),
                    new RoleEntity(RoleType.User)
                };

                await Roles.AddRangeAsync(roles);
                await SaveChangesAsync();
            }
        }

        private async Task SeedPermissionsAsync()
        {
            if (!await Permissions.AnyAsync())
            {
                var permissions = new[]
                {
                    new PermissionEntity(PermissionType.Read),
                    new PermissionEntity(PermissionType.Manage),
                    new PermissionEntity(PermissionType.All)
                };

                await Permissions.AddRangeAsync(permissions);
                await SaveChangesAsync();
            }
        }

        private async Task SeedRolesAndPermissionsAsync()
        {
            var adminRole = await Roles.Include(ar => ar.Permissions).FirstAsync(r => r.Name == RoleType.Admin);
            var userRole = await Roles.Include(ur => ur.Permissions).FirstAsync(r => r.Name == RoleType.User);

            var all = await Permissions.FirstAsync(p => p.Name == PermissionType.All);
            var manage = await Permissions.FirstAsync(p => p.Name == PermissionType.Manage);
            var read = await Permissions.FirstAsync(p => p.Name == PermissionType.Read);

            if (!adminRole.Permissions.Any(p => p.Id == all.Id)) adminRole.AddPermission(all);
            if (!adminRole.Permissions.Any(p => p.Id == manage.Id)) adminRole.AddPermission(manage);
            if (!adminRole.Permissions.Any(p => p.Id == read.Id)) adminRole.AddPermission(read);

            if (!userRole.Permissions.Any(p => p.Id == read.Id)) userRole.AddPermission(read);

            await SaveChangesAsync();
        }

        private async Task SeedAdminAsync()
        {
            var adminEmailValue = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
            var adminPasswordValue = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            var adminEmail = new Email(adminEmailValue ?? string.Empty);

            if (string.IsNullOrWhiteSpace(adminEmailValue) || string.IsNullOrWhiteSpace(adminPasswordValue))
                return;

            var existingAdmin = await Users.FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(adminEmail));
            if (existingAdmin != null)
                return;

            var adminRole = await Roles.FirstAsync(r => r.Name == RoleType.Admin);

            var hashedPassword = _passwordHasher.HashPassword(adminPasswordValue);

            var adminUser = UserEntity.Create(
                emailValue: adminEmailValue,
                phoneNumberValue: null,
                passwordValue: hashedPassword,
                roleId: adminRole.Id
            );

            await Users.AddAsync(adminUser);
            await SaveChangesAsync();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<IAggregateRoot>()
                .Where(x => x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresEfWriteAccessDbContext).Assembly);
        }
    }
}
