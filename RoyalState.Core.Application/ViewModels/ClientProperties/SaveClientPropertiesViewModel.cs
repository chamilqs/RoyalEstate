using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.ClientProperties
{
    public class SaveClientPropertiesViewModel
    {
        public int Id { get; set; }
        [Required]
        public int PropertyId { get; set; }
        [Required]
        public int ClientId { get; set; }
    }
}
