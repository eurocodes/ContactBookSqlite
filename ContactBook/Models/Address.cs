using System;

namespace ContactBook.Models
{
    public class Address
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ContactId { get; set; }
        public string UserId { get; set; }

    }
}