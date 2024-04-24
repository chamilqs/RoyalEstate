namespace RoyalState.Core.Domain.Entities
{
    public class PropertyImprovement
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }  // Esto estaba mal se cambio de PropertyImprovementID a ImprovementID

        //Navigation Properties
        public Property? Property { get; set; }
        public Improvement? Improvement { get; set; }
    }
}
