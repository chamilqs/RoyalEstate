namespace RoyalState.Core.Application.ViewModels.Property
{
    public class PropertyViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int PropertyTypeId { get; set; }
        public int SaleTypeId { get; set; }
        public double Price { get; set; }
        public double Meters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Description { get; set; }

        // Agent details
        public int AgentId { get; set; }
        public string AgentFirstName { get; set; }
        public string AgentLastName { get; set; }
        public string AgentName => $"{AgentFirstName} {AgentLastName}";
        public string AgentPhone { get; set; }
        public string AgentEmail { get; set; }
        public string AgentImage { get; set; }


        public List<string>? PropertyImages { get; set; }
        public List<string>? Improvements { get; set; }

        // public ICollection<PropertyImageViewModel> PropertyImages { get; set; }

    }
}
