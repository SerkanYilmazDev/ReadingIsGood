using System;
using System.Threading.Tasks;
using Shared.MessageHandlers;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using Services.Customers.Data;
using Services.Orders.Data;

namespace Services.Orders.Events.Handlers
{
    public class BooksReserveFailedHandler : IEventHandler<BooksReserveFailed>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly OrderDBContext _dbContext;
        private readonly ILogger<BooksReserveFailedHandler> _logger;

        public BooksReserveFailedHandler(IBusPublisher busPublisher,
                                         OrderDBContext dbContext,
                                         ILogger<BooksReserveFailedHandler> logger)
        {
            _logger = logger;
            _busPublisher = busPublisher;
            _dbContext = dbContext;
        }
        public async Task HandleAsync(BooksReserveFailed _event, ICorrelationContext context)
        {
            var order = await _dbContext.Orders.FindAsync(_event.OrderId);
            order.Status = OrderStatus.Failed;
            order.UpdateDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : Order failed. CorrelationId: {context.CorrelationId}");
            await _busPublisher.PublishAsync(new OrderFailed(_event.OrderId), context);
        }
    }
}