using System;

namespace Shared.RabbitMq
{
    public interface ICorrelationContext
    {
        Guid CorrelationId { get; }
        Guid UserId { get; }
    }
}