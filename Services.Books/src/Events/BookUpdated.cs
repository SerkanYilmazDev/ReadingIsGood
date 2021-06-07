using Shared.Messages;
using System;

namespace Services.Books.Events
{
    public class BookUpdated : IEvent
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public BookUpdated(Guid id, string name, decimal price, int quantity)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
        }
    }
}
