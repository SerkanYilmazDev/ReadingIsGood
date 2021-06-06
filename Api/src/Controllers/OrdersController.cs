using System.Threading.Tasks;
using Api.Commands;
using Shared.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers
{
    [Route("/order-api")]
    public class OrdersController : BaseController
    {
        public OrdersController(IBusPublisher busPublisher) : base(busPublisher)
        {

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            var context = CorrelationContext.Create(Guid.NewGuid(), CurrentUser.UserId);

            await BusPublisher.SendAsync(command, context);

            return Accepted(context);
        }
    }
}
