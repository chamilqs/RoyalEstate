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
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Developer> Developers { get; set; }
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
            modelBuilder.Entity<Developer>().ToTable("Developers");
            modelBuilder.Entity<Admin>().ToTable("Admins");
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
            modelBuilder.Entity<Developer>().HasKey(developer => developer.Id);
            modelBuilder.Entity<Admin>().HasKey(admin => admin.Id);
            modelBuilder.Entity<ClientProperties>().HasKey(cProperty => cProperty.Id);
            modelBuilder.Entity<Improvement>().HasKey(improvement => improvement.Id);
            modelBuilder.Entity<Property>().HasKey(property => property.Id);
            modelBuilder.Entity<PropertyImage>().HasKey(pImage => pImage.Id);
            modelBuilder.Entity<PropertyImprovement>().HasKey(pImprovement => pImprovement.Id);
            modelBuilder.Entity<PropertyType>().HasKey(propertyTypes => propertyTypes.Id);
            modelBuilder.Entity<SaleType>().HasKey(saleTypes => saleTypes.Id);
            #endregion

            #region Relationships

            #region Agent

            modelBuilder.Entity<Agent>()
                .HasMany<Property>(a => a.Properties)
                .WithOne(prop => prop.Agent)
                .HasForeignKey(prop => prop.AgentId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Client

            modelBuilder.Entity<Client>()
                .HasMany<Property>(a => a.FavoriteProperties)
                .WithMany(prop => prop.Clients)
                .UsingEntity<ClientProperties>(
                l => l.HasOne<Property>(p => p.Property).WithMany(p => p.ClientProperties),
                p => p.HasOne<Client>(c => c.Client).WithMany(c => c.ClientProperties));


            #endregion

            #region Property

            modelBuilder.Entity<Property>()
                .HasMany<PropertyImage>(p => p.PropertyImages)
                .WithOne(pi => pi.Property)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
               .HasMany<Improvement>(p => p.Improvements)
               .WithMany(pi => pi.Properties)
               .UsingEntity<PropertyImprovement>(
                  l => l.HasOne<Improvement>(imp => imp.Improvement).WithMany(i => i.PropertyImprovements),
                  p => p.HasOne<Property>(p => p.Property).WithMany(p => p.PropertyImprovements));


            #endregion

            #region PropertyType

            modelBuilder.Entity<PropertyType>()
                .HasMany<Property>(p => p.Properties)
                .WithOne(prop => prop.PropertyType)
                .HasForeignKey(prop => prop.PropertyTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region SalesType

            modelBuilder.Entity<SaleType>()
                 .HasMany<Property>(s => s.Properties)
                 .WithOne(prop => prop.SaleType)
                 .HasForeignKey(prop => prop.SaleTypeId)
                 .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #endregion

            #region "Property configuration"

            #region Admin
            modelBuilder.Entity<Admin>()
                .Property(admin => admin.UserId)
                .IsRequired();
            modelBuilder.Entity<Admin>()
                .Property(admin => admin.Identification)
                .IsRequired();

            #endregion

            #region Developer

            modelBuilder.Entity<Developer>()
               .Property(developer => developer.UserId)
               .IsRequired();
            modelBuilder.Entity<Developer>()
                .Property(developer => developer.Identification)
                .IsRequired();
            #endregion

            #region Agent
            modelBuilder.Entity<Agent>()
             .Property(agent => agent.UserId)
             .IsRequired();
            #endregion

            #region Client
            modelBuilder.Entity<Client>()
          .Property(client => client.UserId)
          .IsRequired();

            #endregion

            #region Join Tables

            #region ClientProperties

            modelBuilder.Entity<ClientProperties>()
             .Property(cp => cp.ClientId)
             .IsRequired();

            modelBuilder.Entity<ClientProperties>()
           .Property(cp => cp.PropertyId)
           .IsRequired();

            #endregion

            #region PropertyImprovements
            modelBuilder.Entity<PropertyImprovement>()
        .Property(propImprovement => propImprovement.PropertyId)
        .IsRequired();
            modelBuilder.Entity<PropertyImprovement>()
        .Property(propImprovement => propImprovement.ImprovementId)
        .IsRequired();

            #endregion

            #endregion

            #region TypeBaseEntities

            #region Improvement

            modelBuilder.Entity<Improvement>()
               .Property(improvement => improvement.Name)
               .IsRequired()
               .HasMaxLength(50);

            modelBuilder.Entity<Improvement>()
               .Property(improvement => improvement.Description)
               .IsRequired()
               .HasMaxLength(150);

            #endregion

            #region PropertyType

            modelBuilder.Entity<PropertyType>()
              .Property(pt => pt.Name)
              .IsRequired()
              .HasMaxLength(50);

            modelBuilder.Entity<PropertyType>()
               .Property(pt => pt.Description)
               .IsRequired()
              .HasMaxLength(150);

            #endregion

            #region SaleType

            modelBuilder.Entity<SaleType>()
             .Property(st => st.Name)
             .IsRequired()
             .HasMaxLength(50);

            modelBuilder.Entity<SaleType>()
               .Property(st => st.Description)
               .IsRequired()
               .HasMaxLength(150);

            #endregion

            #endregion

            #region PropertyImage

            modelBuilder.Entity<PropertyImage>()
         .Property(pi => pi.PropertyId)
         .IsRequired();

            #endregion

            #region Property

            modelBuilder.Entity<Property>()
         .Property(property => property.Code)
         .IsRequired();

            modelBuilder.Entity<Property>()
        .Property(property => property.PropertyTypeId)
        .IsRequired();

            modelBuilder.Entity<Property>()
        .Property(property => property.SaleTypeId)
        .IsRequired();

            modelBuilder.Entity<Property>()
        .Property(property => property.Price)
        .IsRequired();

            modelBuilder.Entity<Property>()
        .Property(property => property.Meters)
        .IsRequired();

            modelBuilder.Entity<Property>()
        .Property(property => property.Bedrooms)
        .IsRequired();

            modelBuilder.Entity<Property>()
        .Property(property => property.Bathrooms)
        .IsRequired();

            modelBuilder.Entity<Property>()
        .Property(property => property.Description)
        .IsRequired();

            modelBuilder.Entity<Property>()
        .Property(property => property.AgentId)
        .IsRequired();

            #endregion

            #endregion

            #region HasData
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    UserId = "2d124d85-4239-4c56-b1a8-b5a59c2c7d12",
                    Identification = "402-3698447-8",
                    CreatedBy = "DefaultAppUser",
                    CreatedDate = DateTime.Now,
                });

            modelBuilder.Entity<Developer>().HasData(
                new Developer
                {
                    Id = 1,
                    UserId = "0267618d-1d2b-41ae-b467-cbf9cd3fe956",
                    Identification = "402-6328445-9",
                    CreatedBy = "DefaultAppUser",
                    CreatedDate = DateTime.Now,
                });

            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    UserId = "9a0c2574-2bfb-47cf-a311-a9442fa83a0c",
                    ImageUrl = "https://img.freepik.com/free-photo/handsome-bearded-guy-posing-against-white-wall_273609-20597.jpg",
                    CreatedBy = "DefaultAppUser",
                    CreatedDate = DateTime.Now,
                });

            modelBuilder.Entity<Agent>().HasData(
                new Agent
                {
                    Id = 1,
                    UserId = "34796422-cda8-4aa2-bc8a-cdc567efae06",
                    ImageUrl = "https://mission.org/wp-content/uploads/2019/07/0-AfrrZIuFCim8RNNb.jpg",
                    CreatedBy = "DefaultAppUser",
                    CreatedDate = DateTime.Now,
                });
            #endregion
        }
    }
}
