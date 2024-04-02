using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Agent : BaseEntity
    {
        public string UserId { get; set; }
        // Navigation Properties
        public ICollection<Property>? Properties { get; set; }
    }
}
