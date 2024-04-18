using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.PropertyImage
{
    public class SavePropertyImageViewModel
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must enter a valid PropertyId.")]
        public int PropertyId { get; set; }
        [Required(ErrorMessage = "You must enter an Url for the property image.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ImageUrl { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
