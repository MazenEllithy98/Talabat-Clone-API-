using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class ApplicationIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Mazen Ellithy",
                    Email = "mazen.a.ellethy@gmail.com",
                    UserName = "MazenEllithy",
                    PhoneNumber = "01100300506",
                };
                await userManager.CreateAsync(user , "P@$$W0rd");
            }
        }
    }
}
