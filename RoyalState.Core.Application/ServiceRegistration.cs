using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RoyalState.Core.Application.Interfaces.Services;
using RoyalState.Core.Application.Services;
using System.Reflection;

namespace RoyalState.Core.Application
{
    //Design pattern --> Decorator - Extensions methods
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            #region "Service"
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IClientPropertiesService, ClientPropertiesService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IPropertyImageService, PropertyImageService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<IPropertyImprovementService, PropertyImprovementService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IFileService, FileService>();
            #endregion
        }
    }
}
