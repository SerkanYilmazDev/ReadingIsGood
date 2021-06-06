using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Threading;
using Services.Identity.Data;
using MediatR;
using Services.Identity.Events;
using Shared.RabbitMq;
using Microsoft.EntityFrameworkCore;

namespace Services.Identity.Commands.Handlers
{
    public class RegisterUserCommandHandler : AsyncRequestHandler<RegisterUserCommand>
    {
        private readonly IdentityDBContext _dbContext;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly IBusPublisher _busPublisher;

        public RegisterUserCommandHandler(IdentityDBContext dbContext,
                                    ILogger<RegisterUserCommandHandler> logger,
                                    IBusPublisher busPublisher)
        {
            _logger = logger;
            _busPublisher = busPublisher;
            _dbContext = dbContext;
        }
        protected override async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            var isEmailExist = _dbContext.Users.AsNoTracking().Any(s => s.Email == command.Email);
            if (isEmailExist)
            {
                _logger.LogError($"[Local Transaction] : Email is already exist. {command.Email}");
                throw new ApplicationException("Email is already exist.");
            }

            _dbContext.Users.Add(new User
            {
                Id = command.Id,
                Password = command.Password,
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Address = command.Address
            });

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : User created.");

            await _busPublisher.PublishAsync(new UserCreated(command.Id, command.Email, command.FirstName,
                 command.LastName), null);
        }
    }
}
