using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Improvement : TypeBaseEntity
    {
        // Navigation Properties
        public ICollection<PropertyImprovement>? PropertyImprovements { get; set; }
    }
}
