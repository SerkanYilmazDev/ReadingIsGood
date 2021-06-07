using System.Threading.Tasks;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using Services.Customers.Data;
using Services.Books.Data;
using Services.Books.Events;
using System;
using System.Linq;
using MediatR;
using System.Threading;

namespace Services.Books.Commands.Handlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Guid>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly BookDBContext _dbContext;
        private readonly ILogger<UpdateBookCommandHandler> _logger;

        public UpdateBookCommandHandler(IBusPublisher busPublisher,
              BookDBContext dbContext,
              ILogger<UpdateBookCommandHandler> logger)
        {
            _busPublisher = busPublisher;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books.FindAsync(command.Id);
            if (book == null)
            {
                _logger.LogError($"[Local Transaction] : Book is null {command.Id}");
                throw new ApplicationException($"Book is null. ");
            }

            book.Price = command.Price;
            book.Quantity = command.Quantity;

            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : Book updated.");

            await _busPublisher.PublishAsync(new BookUpdated(
                book.Id, book.Name, command.Price, command.Quantity), null);

            return book.Id;
        }
    }
}
