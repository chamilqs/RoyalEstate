using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Core.Application.ViewModels.Types
{
    public class TypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PropertiesQuantity { get; set; }
        public List<PropertyViewModel>? Properties { get; set; }
    }
}
