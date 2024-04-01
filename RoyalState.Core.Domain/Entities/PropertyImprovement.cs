namespace RoyalState.Core.Domain.Entities
{
    public class PropertyImprovement
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int PropertyImprovementId { get; set; }

        //Navigation Properties
        public ICollection<Property>? Properties { get; set; }
        public ICollection<Improvement>? Improvements { get; set; }
    }
}
