using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Core.Domain.Entities;
using RoyalState.Infrastructure.Persistence.Contexts;

namespace RoyalState.Infrastructure.Persistence.Repositories
{
    internal class ClientPropertiesRepository : GenericRepository<ClientProperties>, IClientPropertiesRepository
    {
        private readonly ApplicationContext _dbContext;

        public ClientPropertiesRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
