using ContactBook.ModelDto;
using ContactBook.Models;
using System.Threading.Tasks;

namespace ContactBook.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(RegisterUserDto user);
        Task<bool> MakeRegular(User user);
        Task<bool> MakeAdmin(User user);
        User GetUser(string email);
    }
}
