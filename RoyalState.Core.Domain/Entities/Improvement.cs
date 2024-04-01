using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Improvement : TypeBaseEntity
    {
        // Navigation Properties
        public ICollection<Property>? Properties { get; set; }
        public ICollection<PropertyImprovement>? PropertyImprovements { get; set; }
    }
}
