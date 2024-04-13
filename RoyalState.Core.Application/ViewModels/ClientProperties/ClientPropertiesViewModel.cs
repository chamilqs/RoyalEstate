using RoyalState.Core.Application.ViewModels.Client;
using RoyalState.Core.Application.ViewModels.Property;

namespace RoyalState.Core.Application.ViewModels.ClientProperties
{
    public class ClientPropertiesViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public PropertyViewModel Property { get; set; }
        public int ClientId { get; set; }
        public ClientViewModel Client { get; set; }
    }
}
