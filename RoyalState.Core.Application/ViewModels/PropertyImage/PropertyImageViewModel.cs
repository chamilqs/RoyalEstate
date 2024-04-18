
namespace RoyalState.Core.Application.ViewModels.PropertyImage
{
    public class PropertyImageViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ImageUrl { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
