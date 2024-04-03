using Microsoft.AspNetCore.Identity;
using RoyalState.Core.Application.Enums;
using RoyalState.Infrastructure.Identity.Entities;

namespace RoyalState.Infrastructure.Identity.Seeds
{
    public static class DefaulDeveloperUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultUser = new() 
            {
                UserName = "devuser",
                Email = "devuser@email.com",
                FirstName = "James",
                LastName = "Doe",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            if(userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123P4$$w0rd!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
        }
    }
}
