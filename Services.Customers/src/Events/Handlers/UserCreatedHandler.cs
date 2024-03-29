using System.Threading.Tasks;
using Shared.MessageHandlers;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using Services.Customers.Data;
using Services.Customers.Events;

namespace Services.Customers.Handlers
{
    public class UserCreatedHandler : IEventHandler<UserCreated>
    {
        private readonly CustomerDBContext _dbContext;
        private readonly ILogger<UserCreatedHandler> _logger;

        public UserCreatedHandler(CustomerDBContext dbContext,
                                  ILogger<UserCreatedHandler> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task HandleAsync(UserCreated _event, ICorrelationContext context)
        {
            _dbContext.Customers.Add(new Customer
            {
                Id = _event.Id,
                Email = _event.Email,
                FirstName = _event.FirstName,
                LastName = _event.LastName,
            });

            _dbContext.Baskets.Add(new Basket
            {
                CustomerId = _event.Id
            });

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : Customer created.");
        }
    }
}