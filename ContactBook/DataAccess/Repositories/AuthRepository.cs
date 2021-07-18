using ContactBook.Data;
using ContactBook.DataAccess.Interfaces;
using ContactBook.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.DataAccess.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ContactBookDbContext _dbContext;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager, ContactBookDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }
        public async Task<User> Login(string email, string password, bool rememberme)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: rememberme, true);
            if (!result.Succeeded)
                return null;
            return user;
        }

        public async Task<ICollection<string>> GetUserRoles(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
