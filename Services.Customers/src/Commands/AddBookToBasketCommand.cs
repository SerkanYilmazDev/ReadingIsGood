
using System;
using MediatR;

namespace Services.Customers.Commands
{
    public class AddBookToBasketCommand : IRequest
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
    }
}