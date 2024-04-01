using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Agent : BaseEntity
    {
        // Navigation Properties
        public ICollection<Property>? Properties { get; set; }
    }
}
