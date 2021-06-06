using System.Threading.Tasks;
using Shared.MessageHandlers;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Services.Notifications.Events.Handlers
{
    public class OrderCompletedHandler : IEventHandler<OrderCompleted>
    {
        private readonly ILogger<OrderCompletedHandler> _logger;
        public OrderCompletedHandler(ILogger<OrderCompletedHandler> logger)
        {
            _logger = logger;
        }
        public async Task HandleAsync(OrderCompleted _event, ICorrelationContext context)
        {
            // Order completed sms/mail/push...
            Thread.Sleep(5);

            _logger.LogInformation($"[Local Transaction] : Notification sent for Created Order. CorrelationId: {context.CorrelationId}");
            await Task.CompletedTask;
        }
    }
}