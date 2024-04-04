using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Domain.Entities;
using RoyalState.Infrastructure.Persistence.Contexts;

namespace RoyalState.Infrastructure.Persistence.Repositories
{
    public class SalesTypeRepository : GenericRepository<SaleType>, ISalesTypeRepository
    {
        private readonly ApplicationContext _dbContext;

        public SalesTypeRepository(ApplicationContext dbContext) : base (dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
