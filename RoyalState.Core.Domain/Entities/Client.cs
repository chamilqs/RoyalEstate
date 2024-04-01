using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Client : BaseEntity
    {
        // Navigation Properties
        public ICollection<Property>? FavoriteProperties { get; set; }
        public ICollection<ClientProperties>? ClientProperties { get; set; }
    }
}
