using System;
using Shared.Messages;

namespace Api.Commands
{
    [MessageNamespace("orders")]
    public class CreateOrderCommand : ICommand
    {
        public Guid Id { get; private set; }

        public CreateOrderCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}