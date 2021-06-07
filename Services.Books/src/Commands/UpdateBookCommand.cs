using System;
using MediatR;

namespace Services.Books.Commands
{
    public class UpdateBookCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
