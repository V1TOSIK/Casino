using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Domain.Results;
using SharedKernel.Enums;
using User.Core.Application.Ports;

namespace User.Core.Application.User.Commands.Delete
{
    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCommandHandler> _logger;

        public DeleteCommandHandler(
            IUserRepository userRepository, 
            IUserUnitOfWork unitOfWork, 
            ILogger<DeleteCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

            if (user == null)
            {
                _logger.LogInformation("User with Id: {userId} not found.", command.UserId);
                return Result.Failure("User not found", ErrorCode.NotFound);
            }

            user.Delete();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
