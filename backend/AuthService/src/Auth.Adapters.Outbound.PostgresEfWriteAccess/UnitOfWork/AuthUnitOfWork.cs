using Auth.Core.Application.Ports;
using SharedKernel.UnitOfWork;

namespace Auth.Adapters.Outbound.PostgresEfWriteAccess.UnitOfWork
{
    public class AuthUnitOfWork : IAuthUnitOfWork
    {
        private readonly IUnitOfWork<PostgresEfWriteAccessDbContext> _unitOfWork;
        public AuthUnitOfWork(IUnitOfWork<PostgresEfWriteAccessDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
            => await _unitOfWork.SaveChangesAsync(cancellationToken);

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
            => await _unitOfWork.ExecuteInTransactionAsync(action, cancellationToken);
    }
}
