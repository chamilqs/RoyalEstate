using System.ComponentModel.DataAnnotations;

namespace RoyalState.Core.Application.ViewModels.Users
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "You mus enter an email.")]
        [DataType(DataType.Text)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Credential { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required(ErrorMessage = "You must enter a password.")]
        [DataType(DataType.Password)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Password { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
