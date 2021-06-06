using System;
using Shared.Messages;

namespace Services.Customers.Events
{
    [MessageNamespace("books")]
    public class BookCreated : IEvent
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public BookCreated(Guid id, string name, decimal price, int quantity)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
        }
    }
}