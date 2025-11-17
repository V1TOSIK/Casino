using Microsoft.EntityFrameworkCore;

namespace SharedKernel.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken) => await _dbContext.SaveChangesAsync(cancellationToken);

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                if (_dbContext.Database.CurrentTransaction != null)
                {
                    await action();
                    return;
                }

                await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await action();
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }
    }
}
