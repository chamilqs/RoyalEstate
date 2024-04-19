using Microsoft.AspNetCore.Identity;
using RoyalState.Core.Application.Enums;
using RoyalState.Infrastructure.Identity.Entities;

namespace RoyalState.Infrastructure.Identity.Seeds
{
    public static class DefaulAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultUser = new()
            {
                Id = "9e29339b-a30c-4478-bab4-fe745c4d5e75",
                UserName = "adminuser",
                Email = "adminuser@email.com",
                FirstName = "Jake",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123P4$$w0rd!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
        }
    }
}
