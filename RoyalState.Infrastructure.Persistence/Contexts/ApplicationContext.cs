using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RoyalState.Core.Domain.Common;
using RoyalState.Core.Domain.Entities;

namespace RoyalState.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAssesor) : base(options) { }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientProperties> ClientProperties { get; set; }
        public DbSet<Improvement> Improvements { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyImprovement> PropertyImprovements { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Fluent API
            #region Tables
            modelBuilder.Entity<Agent>().ToTable("Agents");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<ClientProperties>().ToTable("ClientProperties");
            modelBuilder.Entity<Improvement>().ToTable("Improvements");
            modelBuilder.Entity<Property>().ToTable("Properties");
            modelBuilder.Entity<PropertyImage>().ToTable("PropertyImages");
            modelBuilder.Entity<PropertyImprovement>().ToTable("PropertyImprovements");
            modelBuilder.Entity<PropertyType>().ToTable("PropertyTypes");
            modelBuilder.Entity<SaleType>().ToTable("SaleTypes");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<Agent>().HasKey(agent => agent.Id);
            modelBuilder.Entity<Client>().HasKey(client => client.Id);
            modelBuilder.Entity<ClientProperties>().HasKey(cProperty => cProperty.Id);
            modelBuilder.Entity<Improvement>().HasKey(improvement => improvement.Id);
            modelBuilder.Entity<Property>().HasKey(property => property.Id);
            modelBuilder.Entity<PropertyImage>().HasKey(pImage => pImage.Id);
            modelBuilder.Entity<PropertyImprovement>().HasKey(pImprovement => pImprovement.Id);
            modelBuilder.Entity<PropertyType>().HasKey(propertyTypes => propertyTypes.Id);
            modelBuilder.Entity<SaleType>().HasKey(saleTypes => saleTypes.Id);
            #endregion

            #region Relationships

            #endregion

            #region "Property configuration"

            #endregion
        }
    }
}
