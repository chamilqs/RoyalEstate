using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.Users
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "You mus enter an email.")]
        [DataType(DataType.Text)]
        public string Credential { get; set; }

        [Required(ErrorMessage = "You must enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
