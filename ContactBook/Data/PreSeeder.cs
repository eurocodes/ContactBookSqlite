using ContactBook.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Data
{
    public static class PreSeeder
    {
        public static async Task<bool> SeedRole(ContactBookDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleList = { "Admin", "Regular" };

            if (!context.Roles.Any())
            {
                for (int i = 0; i < roleList.Length; i++)
                {
                    var role = new IdentityRole(roleList[i]);
                    await roleManager.CreateAsync(role);
                }
                return true;
            }
            return false;
        }
    }
}
