namespace RoyalState.Core.Domain.Entities
{
    public class ClientProperties
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int ClientId { get; set; }

        //Navigation Properties
        public ICollection<Property>? Properties { get; set; }
        public ICollection<Client>? Clients { get; set; }
    }
}
