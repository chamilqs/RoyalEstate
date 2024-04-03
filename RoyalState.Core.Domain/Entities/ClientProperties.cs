namespace RoyalState.Core.Domain.Entities
{
    public class ClientProperties
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int ClientId { get; set; }

        //Navigation Properties
        public Property? Property { get; set; }
        public Client? Client { get; set; }
    }
}
