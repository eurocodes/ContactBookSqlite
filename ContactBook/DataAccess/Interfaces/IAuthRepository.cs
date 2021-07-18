using ContactBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.DataAccess.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Login(string email, string password, bool rememberme);
        Task<ICollection<string>> GetUserRoles(User user);
    }
}
