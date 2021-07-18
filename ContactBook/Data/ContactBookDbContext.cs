using ContactBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Data
{
    public class ContactBookDbContext : IdentityDbContext<User>
    {
        public ContactBookDbContext(DbContextOptions<ContactBookDbContext> options): base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
