using System.Threading.Tasks;
using Shared.MessageHandlers;
using Shared.RabbitMq;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Services.Notifications.Events.Handlers
{
    public class OrderFailedHandler : IEventHandler<OrderFailed>
    {
        private readonly ILogger<OrderFailedHandler> _logger;
        public OrderFailedHandler(ILogger<OrderFailedHandler> logger)
        {
            _logger = logger;
        }
        public async Task HandleAsync(OrderFailed _event, ICorrelationContext context)
        {
            // Order failed sms/mail/push...
            Thread.Sleep(5);

            _logger.LogInformation($"[Local Transaction] : Notification sent for Failed Order. CorrelationId: {context.CorrelationId}");
            await Task.CompletedTask;
        }
    }
}