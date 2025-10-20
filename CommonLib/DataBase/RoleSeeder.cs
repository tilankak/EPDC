using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.DataBase
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<CustomRoleStore>>();
            string[] roles = { "Admin", "User" };

            //foreach (var role in roles)
            //{
            //    if (!await roleManager.RoleExistsAsync(role))
            //    {
            //        await roleManager.CreateAsync(new IdentityRole(role));
            //    }
            //}
        }
    }
}
