using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Domain.Entities;
using RoyalState.Infrastructure.Persistence.Contexts;

namespace RoyalState.Infrastructure.Persistence.Repositories
{
    public class PropertyImprovementRepository : GenericRepository<PropertyImprovement>, IPropertyImprovementRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyImprovementRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}