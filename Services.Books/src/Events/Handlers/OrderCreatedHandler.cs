using System;
using System.Threading.Tasks;
using Shared.MessageHandlers;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using Services.Customers.Data;

namespace Services.Books.Events.Handlers
{
    public class OrderCreatedHandler : IEventHandler<OrderCreated>
    {
        private readonly BookDBContext _dbContext;
        private readonly IBusPublisher _busPublisher;
        private readonly ILogger<OrderCreatedHandler> _logger;

        public OrderCreatedHandler(BookDBContext dbContext,
                            IBusPublisher busPublisher,
                            ILogger<OrderCreatedHandler> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
            _busPublisher = busPublisher;
        }
        public async Task HandleAsync(OrderCreated _event, ICorrelationContext context)
        {
            var isReserved = true;

            foreach ((Guid bookId, int quantity) in _event.Books)
            {
                var book = await _dbContext.Books.FindAsync(bookId);

                if (book == null)
                {
                    isReserved = false;
                    _logger.LogInformation($"[Local Transaction] : Book '{bookId}' not found. CorrelationId: {context.CorrelationId}");
                    break;
                }

                if (quantity > book.Quantity)
                {
                    isReserved = false;
                    _logger.LogInformation($"[Local Transaction] : Not available {quantity} {book.Name}. CorrelationId: {context.CorrelationId}");
                    break;
                }
                else
                {
                    book.Quantity -= quantity;
                    book.UpdateDate = DateTime.Now;
                }
            }

            if (isReserved)
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"[Local Transaction] : Books reserved. CorrelationId: {context.CorrelationId}");
                await _busPublisher.PublishAsync(new BooksReserved(_event.Id, _event.Books), context);
            }
            else
            {
                _logger.LogInformation($"[Local Transaction] : Books could not be reserved. CorrelationId: {context.CorrelationId}");
                await _busPublisher.PublishAsync(new BooksReserveFailed(_event.Id), context);
            }
        }
    }
}