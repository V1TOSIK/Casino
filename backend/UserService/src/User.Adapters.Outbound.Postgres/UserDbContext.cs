using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.AggregateRoot;
using User.Core.Domain.Entities;

namespace User.Adapters.Outbound.Postgres
{
    public class UserDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public UserDbContext(
            DbContextOptions<UserDbContext> options,
            IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<UserEntity> Users { get; set; }

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        }
    }
}
