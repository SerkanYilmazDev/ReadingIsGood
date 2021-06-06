using System;
using System.Linq;
using System.Threading.Tasks;
using Shared.MessageHandlers;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using Services.Customers.Data;
using Services.Orders.Data;
using Services.Orders.Events;
using Services.Orders.HttpServices;

namespace Services.Orders.Commands.Handlers
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        private readonly IBusPublisher _busPublisher;
        private readonly ICustomerHttpService _customerHttpService;
        private readonly OrderDBContext _dbContext;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IBusPublisher busPublisher,
               ICustomerHttpService customerHttpService,
               OrderDBContext dbContext,
               ILogger<CreateOrderCommandHandler> logger)
        {
            _busPublisher = busPublisher;
            _customerHttpService = customerHttpService;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task HandleAsync(CreateOrderCommand command, ICorrelationContext context)
        {
            // ! Order service depends on Customer Service.
            // local copy?

            var basket = await _customerHttpService.GetBasket(context.UserId);
            if (basket == null)
                throw new Exception($"Basket not found for customer: {context.UserId}");

            var items = basket.Items.Select(i =>
                new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = command.Id,
                    BookId = i.BookId,
                    Name = i.BookName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList();

            var order = new Order
            {
                Status = OrderStatus.Created,
                Id = command.Id,
                CustomerId = context.UserId,
                Items = items,
                TotalAmount = items.Sum(s => s.TotalPrice)
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"[Local Transaction] : Order created. CorrelationId: {context.CorrelationId}");

            await _busPublisher.PublishAsync(new OrderCreated(
                command.Id, items.ToDictionary(i => i.BookId, i => i.Quantity)), context);
        }
    }
}