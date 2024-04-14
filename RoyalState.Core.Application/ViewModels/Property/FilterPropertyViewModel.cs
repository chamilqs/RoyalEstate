namespace RoyalState.Core.Application.ViewModels.Property
{
    public class FilterPropertyViewModel
    {
        public string Code { get; set; }
        public int SaleTypeId { get; set; }
        public int PropertyTypeId { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }

    }
}
