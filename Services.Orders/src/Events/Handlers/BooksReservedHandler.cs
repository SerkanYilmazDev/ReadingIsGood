using System.Threading.Tasks;
using Shared.MessageHandlers;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using Services.Customers.Data;
using Services.Orders.Data;

namespace Services.Orders.Events.Handlers
{
    public class BooksReservedHandler : IEventHandler<BooksReserved>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly OrderDBContext _dbContext;
        private readonly ILogger<BooksReservedHandler> _logger;

        public BooksReservedHandler(IBusPublisher busPublisher,
                         OrderDBContext dbContext,
                         ILogger<BooksReservedHandler> logger)
        {
            _logger = logger;
            _busPublisher = busPublisher;
            _dbContext = dbContext;
        }
        public async Task HandleAsync(BooksReserved _event, ICorrelationContext context)
        {
            var order = await _dbContext.Orders.FindAsync(_event.OrderId);
            order.Status = OrderStatus.Completed;

            await _dbContext.SaveChangesAsync();

            using (_logger.BeginScope("{OrderId}", order.Id))
            using (_logger.BeginScope("{TotalAmount}", order.TotalAmount))
            {
                _logger.LogInformation($"[Local Transaction] : Order completed. CorrelationId: {context.CorrelationId}");
            }

            await _busPublisher.PublishAsync(new OrderCompleted(_event.OrderId), context);
        }
    }
}