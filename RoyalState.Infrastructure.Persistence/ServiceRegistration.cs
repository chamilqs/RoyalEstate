using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Infrastructure.Persistence.Contexts;
using RoyalState.Infrastructure.Persistence.Repositories;

namespace RoyalState.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            #region "Context Configurations"

            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(opt => opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString, migration => migration.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }

            #endregion

            #region "Repositories"
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IAgentRepository, AgentRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IPropertyImageRepository, PropertyImageRepository>();
            services.AddTransient<ISaleTypeRepository, SaleTypeRepository>();
            services.AddTransient<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddTransient<IImprovementRepository, ImprovementRepository>();

            #endregion
        }
    }
}
