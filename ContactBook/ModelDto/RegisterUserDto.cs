using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.ModelDto
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage ="Firstname required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Lastname require")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email required")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password required")]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }

        [Required(ErrorMessage = "House number is required")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Street name is required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
    }
}
