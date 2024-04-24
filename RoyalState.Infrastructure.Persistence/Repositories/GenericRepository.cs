using Microsoft.EntityFrameworkCore;
using RoyalState.Core.Application.Interfaces.Repositories;
using RoyalState.Infrastructure.Persistence.Contexts;

namespace RoyalState.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationContext _dbContext;

        public GenericRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            await _dbContext.Set<Entity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(Entity entity, int id)
        {
            Entity? entry = await _dbContext.Set<Entity>().FindAsync(id);
            _dbContext.Entry(entry ?? throw new Exception("Entity not found")).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return await _dbContext.Set<Entity>().ToListAsync(); //Deferred execution
        }

        public virtual async Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = _dbContext.Set<Entity>().AsQueryable();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Entity> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Set<Entity>().FindAsync(id);
            return entity ?? throw new Exception($"Entity with id {id} not found");
        }

        public virtual async Task DeleteAsync(Entity entity)
        {
            _dbContext.Set<Entity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
