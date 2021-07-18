using ContactBook.Data;
using ContactBook.DataAccess.Interfaces;
using ContactBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.DataAccess.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactBookDbContext _dbContext;

        public ContactRepository(ContactBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICollection<Contact> GetAllContacts(int page, string userid)
        {
            var contacts = _dbContext.Contacts.Where(c => c.UserId == userid).Skip((page - 1) * 5).Take(5).Include(a => a.Address).ToList();
            if (contacts == null)
                return null;

            return contacts;
        }

        public Contact GetContactById(string Id)
        {
            var contact = _dbContext.Contacts.Where(c => c.Id == Id).Include(a => a.Address).AsNoTracking().FirstOrDefault();
            if (contact == null)
                return null;

            return contact;
        }

        public Contact GetContactByEmail(string email)
        {
            var contact = _dbContext.Contacts.Where(c => c.Email == email).Include(a => a.Address).AsNoTracking().FirstOrDefault();
            if (contact == null)
                return null;

            return contact;
        }

        public async Task<Address> GetAddress(string Id)
        {
            var address = await _dbContext.Addresses.FindAsync(Id);
            if (address == null)
                return null;

            return address;
        }

        public async Task<bool> ContactExist(string contactId)
        {
            var contact = await _dbContext.Contacts.FindAsync(contactId);
            if (contact == null)
                return false;

            return true;

        }

        public async Task<bool> AddContact(Contact contact)
        {
            bool contactExist = await ContactExist(contact.Id);
            if (contactExist)
                return false;

            var contactAdded = _dbContext.Contacts.Add(contact);
            if (contactAdded == null)
                return false;

            return _dbContext.SaveChanges() >= 1;
        }

        public bool UpdateContact(Contact contact)
        {
            var contactAdded = _dbContext.Contacts.Update(contact);
            if (contactAdded == null)
                return false;

            return _dbContext.SaveChanges() >= 1;
        }

        public async Task<bool> DeleteContact(string Id)
        {
            var contact = await _dbContext.Contacts.Where(c => c.Id == Id).Include(a => a.Address).FirstOrDefaultAsync();
            if (contact == null)
                return false;

            var address = await _dbContext.Addresses.FindAsync(contact.Address.Id);
            if (address == null)
                return false;

            _dbContext.Addresses.Remove(address);
            _dbContext.Contacts.Remove(contact);
            return _dbContext.SaveChanges() >= 1;
        }

        public async Task<Contact> Search(string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var contact = await _dbContext.Contacts.Where(c => c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm)
                            || c.Email.Contains(searchTerm) || c.PhoneNumber.Contains(searchTerm)).Include(a => a.Address).FirstOrDefaultAsync();

                if (contact == null)
                    return null;

                return contact;
            }

            return null;
        }

    }
}
