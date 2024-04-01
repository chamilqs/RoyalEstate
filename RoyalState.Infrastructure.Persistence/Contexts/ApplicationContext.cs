using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace RoyalState.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAssesor) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
