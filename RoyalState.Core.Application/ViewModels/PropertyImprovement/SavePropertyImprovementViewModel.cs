using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.PropertyImprovement
{
    public class SavePropertyImprovementViewModel
    {
        public int Id { get; set; }
        [Required]
        public int PropertyId { get; set; }
        [Required]
        public int ImprovementId { get; set; }
    }
}
