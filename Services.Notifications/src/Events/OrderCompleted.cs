using System;
using Shared.Messages;

namespace Services.Notifications.Events
{
    [MessageNamespace("orders")]
    public class OrderCompleted : IEvent
    {
        public Guid Id { get; }
    }
}