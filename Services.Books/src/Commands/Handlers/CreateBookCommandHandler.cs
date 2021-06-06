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
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly BookDBContext _dbContext;
        private readonly ILogger<CreateBookCommandHandler> _logger;

        public CreateBookCommandHandler(IBusPublisher busPublisher,
               BookDBContext dbContext,
               ILogger<CreateBookCommandHandler> logger)
        {
            _busPublisher = busPublisher;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
        {
            var isNameExist = _dbContext.Books.Any(s => s.Name == command.Name);
            if (isNameExist)
            {
                _logger.LogError($"[Local Transaction] : Book name is already exist. {command.Name}");
                throw new ApplicationException($"Book name is already exist. ");
            }

            var book = new Book
            {
                Name = command.Name,
                Id = command.Id,
                Price = command.Price,
                Quantity = command.Quantity
            };

            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : Book created.");

            await _busPublisher.PublishAsync(new BookCreated(
                book.Id, book.Name, book.Price, book.Quantity), null);

            return book.Id;
        }
    }
}