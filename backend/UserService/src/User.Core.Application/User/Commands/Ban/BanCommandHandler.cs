using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;
using User.Core.Application.Ports;

namespace User.Core.Application.User.Commands.Ban
{
    public class BanCommandHandler : IRequestHandler<BanCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly ILogger<BanCommandHandler> _logger;

        public BanCommandHandler(
            IUserRepository userRepository,
            IUserUnitOfWork unitOfWork,
            ILogger<BanCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(BanCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

            if (user == null)
            {
                _logger.LogInformation("User with Id: {userId} not found.", command.UserId);
                return Result.Failure("User not found", ErrorCode.NotFound);
            }

            user.Ban();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
