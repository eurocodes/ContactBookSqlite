using System;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.Models
{
    public class Contact
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        public string Photo { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public virtual Address Address { get; set; }
        public string UserId { get; set; }
    }
}