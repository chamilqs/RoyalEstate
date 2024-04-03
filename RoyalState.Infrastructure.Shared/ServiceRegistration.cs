using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Domain.Settings;
using RoyalState.Infrastructure.Shared.Services;

namespace RoyalState.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
