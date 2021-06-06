using System;
using Shared.Messages;

namespace Services.Customers.Events
{
    [MessageNamespace("orders")]
    public class OrderCompleted : IEvent
    {
        public Guid Id { get; }

        public OrderCompleted(Guid id)
        {
            Id = id;
        }
    }
}