using System;
using Shared.Messages;

namespace Services.Orders.Commands
{
    public class CreateOrderCommand : ICommand
    {
        public Guid Id { get; }

        public CreateOrderCommand(Guid id)
        {
            this.Id = id;
        }
    }
}