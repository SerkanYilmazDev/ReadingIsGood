using System;
using System.Collections.Generic;
using Shared.Messages;

namespace Services.Orders.Events
{
    public class OrderCreated : IEvent
    {
        public Guid Id { get; }
        public IDictionary<Guid, int> Books { get; } // id, quantity

        public OrderCreated(Guid id, IDictionary<Guid, int> books)
        {
            Id = id;
            Books = books;
        }
    }
}