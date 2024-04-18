using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Property : BaseEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Code { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int PropertyTypeId { get; set; }
        public int SaleTypeId { get; set; }
        public double Price { get; set; }
        public double Meters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int AgentId { get; set; }

        // Navigation Properties
        public PropertyType? PropertyType { get; set; }
        public SaleType? SaleType { get; set; }
        public Agent? Agent { get; set; }
        public ICollection<PropertyImage>? PropertyImages { get; set; }
        public ICollection<Improvement>? Improvements { get; set; }
        public ICollection<PropertyImprovement>? PropertyImprovements { get; set; }
        public ICollection<Client>? Clients { get; set; }
        public ICollection<ClientProperties>? ClientProperties { get; set; }
    }
}
