using System;
using System.Collections.Generic;

namespace Services.Orders.Dtos
{
    public class BasketDto
    {
        public Guid CustomerId { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}