using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.Client
{
    public class SaveClientViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You mus enter a UserId.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string UserId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required(ErrorMessage = "You must enter an image Url for the profile picture.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ImageUrl { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
