using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Customers.Data;
using Services.Customers.HttpServices;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Services.Customers.Commands.Handlers
{
    public class AddBookToBasketCommandHandler : AsyncRequestHandler<AddBookToBasketCommand>
    {
        private readonly CustomerDBContext _customerDbContext;
        private readonly IBookHttpService _bookHttpService;
        private readonly ILogger<AddBookToBasketCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddBookToBasketCommandHandler(CustomerDBContext dbContext,
            IBookHttpService bookHttpService,
            ILogger<AddBookToBasketCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookHttpService = bookHttpService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _customerDbContext = dbContext;
        }

        protected override async Task Handle(AddBookToBasketCommand command, CancellationToken cancellation)
        {
            var book = await _customerDbContext.Books.FindAsync(command.BookId);
            if (book == null)
            {
                book = await _bookHttpService.GetAsync(command.BookId);
                if (book == null)
                {
                    throw new Exception($"Book not found. Id: {command.BookId}");
                }
                else
                {
                    await CreateMissingBook(book);
                }
            }

            var userId = Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers["user_id"][0]);

            var basket = await _customerDbContext.Baskets.FirstOrDefaultAsync(q => q.CustomerId == userId);

            if (basket == null)
                throw new Exception($"Basket not found for customer: {userId}");

            var basketItem = await _customerDbContext.BasketItems.FirstOrDefaultAsync(q => q.BookId == command.BookId);

            if (basketItem != null)
            {
                basketItem.Quantity += command.Quantity;
            }
            else
            {
                _customerDbContext.BasketItems.Add(
                    new BasketItem
                    {
                        BasketId = basket.Id,
                        Quantity = command.Quantity,
                        BookId = book.Id,
                        BookName = book.Name,
                        UnitPrice = book.Price
                    });
            }

            await _customerDbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : {command.Quantity} {book.Name} added to basket.");
        }

        private async Task CreateMissingBook(Book book)
        {
            _logger.LogInformation($"Detected missing book.");

            book.UpdateDate = null;
            book.CreateDate = DateTime.Now;

            _customerDbContext.Books.Add(book);
            await _customerDbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : Added missing book: {book.Name}");
        }
    }
}