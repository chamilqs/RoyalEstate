using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.PropertyImage
{
    public class SavePropertyImageViewModel
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must enter a valid PropertyId.")]
        public int PropertyId { get; set; }
        [Required(ErrorMessage = "You must enter an Url for the property image.")]

        public string ImageUrl { get; set; }

    }
}
