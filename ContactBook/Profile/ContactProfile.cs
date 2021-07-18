using AutoMapper;
using ContactBook.ModelDto;
using ContactBook.Models;

namespace ContactBookProfile
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            this.CreateMap<Contact, AddContactDto>();
        }
    }
}
