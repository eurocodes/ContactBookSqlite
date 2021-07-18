using ContactBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.DataAccess.Interfaces
{
    public interface IContactRepository
    {
        Task<bool> AddContact(Contact contact);
        Task<bool> ContactExist(string contactId);
        ICollection<Contact> GetAllContacts(int page, string userid);
        Contact GetContactById(string Id);
        Contact GetContactByEmail(string email);
        bool UpdateContact(Contact contact);
        Task<Address> GetAddress(string Id);
        Task<bool> DeleteContact(string Id);
        Task<Contact> Search(string searchTerm);
    }
}
