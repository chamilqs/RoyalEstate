namespace RoyalState.Core.Domain.Entities
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string ImageUrl { get; set; }

        //Navigation Properties
        public Property? Property { get; set; }
    }
}
