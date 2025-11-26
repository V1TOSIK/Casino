using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using User.Core.Application.Ports;
using User.Core.Domain.Entities;

namespace User.Adapters.Outbound.Postgres.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
