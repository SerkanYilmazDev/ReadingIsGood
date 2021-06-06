using System;
using Shared.Data;

namespace Services.Orders.Data
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid BookId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}