using Shared.Data;

namespace Services.Customers.Data
{
    public class Customer : BaseEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}