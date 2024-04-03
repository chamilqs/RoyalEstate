using RoyalState.Core.Domain.Common;

namespace RoyalState.Core.Domain.Entities
{
    public class Admin : BaseEntity
    {
        public string UserId { get; set; }
        public string Identification { get; set; }
    }
}
