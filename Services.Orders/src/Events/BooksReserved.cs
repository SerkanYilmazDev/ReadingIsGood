using System;
using System.Collections.Generic;
using Shared.Messages;

namespace Services.Orders.Events
{
    [MessageNamespace("books")]
    public class BooksReserved : IEvent
    {
        public Guid OrderId { get; set; }
        public IDictionary<Guid, int> Books { get; set; }

        public BooksReserved(Guid orderId, IDictionary<Guid, int> books)
        {
            OrderId = orderId;
            Books = books;
        }
    }
}