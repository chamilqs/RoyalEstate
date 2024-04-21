using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.Client
{
    public class SaveClientViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You mus enter a UserId.")]

        public string UserId { get; set; }

        [Required(ErrorMessage = "You must enter an image Url for the profile picture.")]

        public string ImageUrl { get; set; }

    }
}
