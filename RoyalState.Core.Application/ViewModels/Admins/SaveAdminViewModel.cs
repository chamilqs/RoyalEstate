using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.Admins
{
    public class SaveAdminViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter a Username")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter a name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter a lastname")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "You must enter a identification")]
        [DataType(DataType.Text)]
        public string Identification { get; set; }

        [Required(ErrorMessage = "You must enter an email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public bool? EmailConfirmed { get; set; }

        public bool? HasError { get; set; }
        public string? Error { get; set; }
    }
}
