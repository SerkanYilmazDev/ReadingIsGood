using System;
using System.Collections.Generic;
using Shared.Data;

namespace Services.Orders.Data
{
    public class Order : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}

