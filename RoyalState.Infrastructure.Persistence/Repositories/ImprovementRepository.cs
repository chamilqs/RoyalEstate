using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Domain.Entities;
using RoyalState.Infrastructure.Persistence.Contexts;

namespace RoyalState.Infrastructure.Persistence.Repositories
{
    public class ImprovementRepository : GenericRepository<Improvement>, IImprovementRepository
    {
        private readonly ApplicationContext _dbContext;

        public ImprovementRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
