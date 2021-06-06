using System;
using Shared.Messages;

namespace Services.Books.Events
{
    public class BooksReserveFailed : IEvent
    {
        public Guid OrderId { get; set; }
        public BooksReserveFailed(Guid orderId)
        {
            this.OrderId = orderId;
        }
    }
}