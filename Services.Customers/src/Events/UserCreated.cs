using System;
using Shared.Messages;

namespace Services.Customers.Events
{
    [MessageNamespace("identity")]
    public class UserCreated : IEvent
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserCreated(Guid id, string email, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}