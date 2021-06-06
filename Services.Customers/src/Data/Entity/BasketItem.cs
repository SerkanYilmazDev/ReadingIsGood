using System;
using Shared.Data;

namespace Services.Customers.Data
{
    public class BasketItem : BaseEntity
    {
        public Guid BasketId { get; set; }
        public Guid BookId { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}