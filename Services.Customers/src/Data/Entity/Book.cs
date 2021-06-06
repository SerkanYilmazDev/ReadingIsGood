using Shared.Data;

namespace Services.Customers.Data
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}