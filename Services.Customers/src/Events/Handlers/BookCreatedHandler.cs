using System.Threading.Tasks;
using Shared.MessageHandlers;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using Services.Customers.Data;
using Services.Customers.Events;

namespace Services.Customers.Handlers
{
    public class BookCreatedHandler : IEventHandler<BookCreated>
    {
        private readonly CustomerDBContext _dbContext;
        private readonly ILogger<BookCreatedHandler> _logger;

        public BookCreatedHandler(CustomerDBContext dbContext,
                                    ILogger<BookCreatedHandler> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task HandleAsync(BookCreated _event, ICorrelationContext context)
        {
            var book = new Book
            {
                Name = _event.Name,
                Id = _event.Id,
                Price = _event.Price
            };

            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : Book mirror created. CorrelationId: {context.CorrelationId}");
        }
    }
}