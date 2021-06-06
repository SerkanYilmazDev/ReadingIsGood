using System;

namespace Services.Orders.Dtos
{
    public class BasketItemDto
    {
        public Guid Id { get; set; }
        public Guid BasketId { get; set; }
        public Guid BookId { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}