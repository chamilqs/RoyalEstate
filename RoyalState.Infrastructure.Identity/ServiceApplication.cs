using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RoyalState.Infrastructure.Identity.Entities;
using RoyalState.Infrastructure.Identity.Seeds;

namespace RoyalState.Infrastructure.Identity
{
    //Design pattern --> Decorator - Extensions methods
    public static class ServiceApplication
    {
        public static async Task AddIdentitySeeds(this IServiceProvider services)
        {
            #region "Identity Seeds"
            using(var scope = services.CreateScope())
            {
                var serviceScope = scope.ServiceProvider;

                try
                {
                    var userManager = serviceScope.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceScope.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsync(roleManager);
                    await DefaulSuperAdminUser.SeedAsync(userManager);
                    await DefaulDeveloperUser.SeedAsync(userManager);
                    await DefaulAdminUser.SeedAsync(userManager);
                    await DefaulAgentUser.SeedAsync(userManager);
                    await DefaulClientUser.SeedAsync(userManager);

                }
                catch (Exception ex) 
                { 
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message.ToString());             
                    Console.ResetColor();
                
                }
            }
            #endregion
        }
    }
}
