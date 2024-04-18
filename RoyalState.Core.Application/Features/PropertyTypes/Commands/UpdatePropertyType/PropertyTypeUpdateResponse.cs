namespace RoyalState.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType
{
    public class PropertyTypeUpdateResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
