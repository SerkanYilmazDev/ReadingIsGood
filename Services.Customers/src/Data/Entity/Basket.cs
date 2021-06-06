using System;
using System.Collections.Generic;
using Shared.Data;

namespace Services.Customers.Data
{
    public class Basket : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}