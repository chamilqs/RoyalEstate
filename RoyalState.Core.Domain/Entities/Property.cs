using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Property : BaseEntity
    {
        public string Code { get; set; }
        public int PropertyTypeId { get; set; }
        public int SaleTypeId { get; set; }
        public double Price { get; set; }
        public double Meters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Description { get; set; }
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
