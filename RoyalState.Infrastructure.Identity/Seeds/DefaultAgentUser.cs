using Microsoft.AspNetCore.Identity;
using RoyalState.Core.Application.Enums;
using RoyalState.Infrastructure.Identity.Entities;

namespace RoyalState.Infrastructure.Identity.Seeds
{
    public static class DefaulAgentUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultUser = new()
            {
                Id = "8e59f571-5d21-4da6-8cd9-afb54d277872",
                UserName = "agentuser",
                Email = "agentuser@email.com",
                FirstName = "Jules",
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
                    await userManager.AddToRoleAsync(defaultUser, Roles.Agent.ToString());
                }
            }
        }
    }
}
