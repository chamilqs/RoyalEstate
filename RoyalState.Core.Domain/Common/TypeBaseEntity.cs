using RoyalState.Core.Domain.Entities;

namespace RoyalState.Core.Domain.Common
{
    public class TypeBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Property>? Properties { get; set; }
    }
}
