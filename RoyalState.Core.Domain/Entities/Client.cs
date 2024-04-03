using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Client : BaseEntity
    {
        public string UserId { get; set; }
        public string ImageUrl { get; set; }

        // Navigation Properties
        public ICollection<Property>? FavoriteProperties { get; set; }
        public ICollection<ClientProperties>? ClientProperties { get; set; }
    }
}
