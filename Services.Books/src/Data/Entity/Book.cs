using Shared.Data;

namespace Services.Books.Data
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}