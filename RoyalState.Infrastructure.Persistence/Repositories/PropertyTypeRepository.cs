using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Domain.Entities;
using RoyalState.Infrastructure.Persistence.Contexts;

namespace RoyalState.Infrastructure.Persistence.Repositories
{
    public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
