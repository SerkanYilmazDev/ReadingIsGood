using System;
using Shared.Messages;

namespace Services.Orders.Events
{
    public class OrderCompleted : IEvent
    {
        public Guid Id { get; }

        public OrderCompleted(Guid id)
        {
            Id = id;
        }
    }
}