namespace RoyalState.Core.Application.ViewModels.SalesTypes
{
    public class SalesTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PropertyViewModel>? Properties { get; set; }
    }
}
