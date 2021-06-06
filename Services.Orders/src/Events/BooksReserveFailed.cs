using System;
using Shared.Messages;

namespace Services.Orders.Events
{
    [MessageNamespace("books")]
    public class BooksReserveFailed : IEvent
    {
        public Guid OrderId { get; set; }
        public BooksReserveFailed(Guid orderId)
        {
            this.OrderId = orderId;
        }
    }
}