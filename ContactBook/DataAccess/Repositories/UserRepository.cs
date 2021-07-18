using ContactBook.Data;
using ContactBook.DataAccess.Interfaces;
using ContactBook.ModelDto;
using ContactBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ContactBookDbContext _dbContext;

        public UserRepository(UserManager<User> userMagager, ContactBookDbContext dbContext)
        {
            _userManager = userMagager;
            _dbContext = dbContext;
        }

        public async Task<User> RegisterUser(RegisterUserDto userDto)
        {
            var dbUser = await _userManager.FindByEmailAsync(userDto.Email);
            if (dbUser != null)
                return null;
            var userToAdd = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                UserName = userDto.Username,
                Address = new Address
                {
                    HouseNumber = userDto.HouseNumber,
                    Street = userDto.Street,
                    City = userDto.City,
                    State = userDto.State,
                    Country = userDto.Country,
                }

            };
            var result = await _userManager.CreateAsync(userToAdd, userDto.Password);
            if (!result.Succeeded)
                return null;

            var user = await _userManager.FindByEmailAsync(userToAdd.Email);
            await MakeAdmin(user);
            await MakeRegular(user);
            if (user != null)
                return user;

            return null;
        }

        public async Task<bool> MakeAdmin(User user)
        {
            var makeAdmin = await _userManager.AddToRoleAsync(user, "Admin");
            if (makeAdmin.Succeeded)
                return true;

            return false;
        }

        public async Task<bool> MakeRegular(User user)
        {
            var makeAdmin = await _userManager.AddToRoleAsync(user, "Regular");
            if (makeAdmin.Succeeded)
                return true;

            return false;
        }

        public User GetUser(string userId)
        {
            var user = _dbContext.Users.Where(c => c.Id == userId).Include(a => a.Address).AsNoTracking().FirstOrDefault();
            if (user == null)
                return null;
            
            return user;
        }
    }
}
