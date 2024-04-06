using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.Agent
{
    public class SaveAgentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You mus enter a UserId.")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "You must enter a Url for the profile picture.")]
        public string ImageUrl { get; set; }
    }
}
