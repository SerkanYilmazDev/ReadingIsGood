using System;
using Shared.Messages;

namespace Services.Notifications.Events
{
    [MessageNamespace("orders")]
    public class OrderFailed : IEvent
    {
        public Guid Id { get; }
    }
}